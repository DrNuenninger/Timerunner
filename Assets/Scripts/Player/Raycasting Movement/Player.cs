﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Sprite))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    private bool crouchIsPressed;
    
    
    public float movespeed = 6f;
    public float sprintSpeedModifier = 2f;
    public float accelerationTimeSprint = 0.4f;
    private float currentSprintSpeed = 0f;
    private float speedSmoothing;


    private Vector3 velocity;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    //TODO change acceleration so that it starts fresh when jumping over a wall you were walking against
    public float accelerationTimeAirborn = 0.2f;
    public float accelerationTimeGrounded = 0.1f;

    //Nur als Vorbereitung für Crouchsliding und speed reduction
    public float crouchSpeedMultiplier = 0.6f;
    public float maxCrouchSlideTime = 2f;
    private float slideTimer = 0f;

    public bool isCrouchSliding = false;
    public bool crouchSlideSlowdown = false;
    


    private float crouchSlideSmoothing;
    //Wallslide variables
    public float wallSlideSpeedMax = 3f;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallJumpLarge;
    public float wallStickTime = 0.25f;
    public float timeToWallUnstick;

    public Controller2D controller;
    

    void Start()
    {        
        //Berechnet die Schwerkraft und Sprunggeschwindigkeit
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + " JumpVelocity: " + maxJumpVelocity);
    }

 
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirectionX = (controller.collissions.left) ? -1 : 1;
        float targetSprintSpeed = (movespeed * sprintSpeedModifier) - movespeed;
        float targetVelocityX;
        float localCrouchSpeedMultiplier;
        
        if (controller.wasCrouchedLastFrame && controller.collissions.below)
        {
            if (isCrouchSliding)
            {
                slideTimer += Time.deltaTime;
                localCrouchSpeedMultiplier = 1f;
                if (slideTimer >= maxCrouchSlideTime)
                {
                    isCrouchSliding = false;
                    crouchSlideSlowdown = true;
                }
            }
            else
            {
                localCrouchSpeedMultiplier = crouchSpeedMultiplier;
            }

            if (crouchSlideSlowdown)
            {
                localCrouchSpeedMultiplier = crouchSpeedMultiplier;
                currentSprintSpeed = 0;
                crouchSlideSlowdown = false;
            }

            if (!isCrouchSliding && !crouchSlideSlowdown && Mathf.Abs(velocity.x) > Mathf.Abs(1*movespeed * crouchSpeedMultiplier))
            {
                isCrouchSliding = true;
                slideTimer = 0f;
            }
        }
        else
        {
            isCrouchSliding = false;
            localCrouchSpeedMultiplier = 1f;
        }
        print("Crouch Multiploer = " + localCrouchSpeedMultiplier);
        //Sprinting when on ground will increase SprintSpeed over time
        //Sprinting while in air, wont increase or decrease SprintSpeed when input in that direction
        //else SprintSpeed will decrease
        if (Input.GetKey(KeyCode.LeftShift)) {

            if (controller.wasCrouchedLastFrame && !isCrouchSliding)
            {
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, 0f, ref speedSmoothing,
                    accelerationTimeSprint / 2);
            }else if (controller.collissions.below && input.x > 0)
            {
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, targetSprintSpeed, ref speedSmoothing,
                    accelerationTimeSprint);
            }else if (controller.collissions.below && input.x < 0)
            {
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, -targetSprintSpeed, ref speedSmoothing,
                    accelerationTimeSprint);
            }else if ((!controller.collissions.below && Mathf.Sign(input.x) == Mathf.Sign(currentSprintSpeed)) || isCrouchSliding)
            {
                //Intended to be empty
            }
            
        }
        else
        {
            currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, 0f, ref speedSmoothing,
                accelerationTimeSprint / 2);
        }
        targetVelocityX = (input.x * movespeed + currentSprintSpeed) * localCrouchSpeedMultiplier;
        //Erlaubt ein momentumbasiertes Bewegunssystem
        if ((controller.collissions.left && input.x < 0) || (controller.collissions.right && input.x > 0))
        {
            velocity.x = 0f;
            currentSprintSpeed = 0f;
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                (controller.collissions.below) ? accelerationTimeGrounded : accelerationTimeAirborn);
        }

        print("Current Vec: " + velocity.x + " Target Vec: "+ targetVelocityX + " SprintSpeed: " + currentSprintSpeed);
        bool wallSliding = false;
        //Überprüft ob die Bedingungen für einen Wallslide vorhanden sind

        if ((controller.collissions.left || controller.collissions.right) && !controller.collissions.below &&
            velocity.y < 0)
        {
            wallSliding = true;
            
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                if (input.x != wallDirectionX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)){ 
            crouchIsPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchIsPressed = false;            
        }
        if(Input.GetKey(KeyCode.LeftControl) && wallSliding)
        {
            crouchIsPressed = false;
        }


       

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirectionX == input.x)
                {
                    velocity.x = -wallDirectionX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirectionX * wallJumpOff.x;
                    velocity.y =  wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirectionX * wallJumpLarge.x;
                    velocity.y = wallJumpLarge.y;
                }
            }

            if (controller.collissions.below && !controller.wasCrouchedLastFrame)
            {
                velocity.y = maxJumpVelocity;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }
        
        velocity.y += gravity * Time.deltaTime;

        //Bewegt den Spieler
        controller.Move(velocity * Time.deltaTime, input, false, crouchIsPressed, wallSliding);

        if (controller.collissions.above || controller.collissions.below)
        {
            velocity.y = 0;
        }

    }
}
