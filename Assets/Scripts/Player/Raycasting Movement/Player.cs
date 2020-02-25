using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Sprite))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    private bool crouchIsPressed;
    
    
    public float movespeed = 6f;
    private Vector3 velocity;
    private float gravity;
    private float jumpvelocity;
    private float velocityXSmoothing;
    //TODO change acceleration so that it starts fresh when jumping over a wall you were walking against
    public float accelerationTimeAirborn = 0.2f;
    public float accelerationTimeGrounded = 0.1f;

    //Nur als Vorbereitung für Crouchsliding und speed reduction
    public float croucheSpeedMultiplier = 0.6f;
    public float crouchSlideMultiplier = 1.4f;
    public float crouchSlideTimeInSeconds = 1.2f;

    public Controller2D controller;
    

    void Start()
    {        
        //Berechnet die Schwerkraft und Sprunggeschwindigkeit
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpvelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + " JumpVelocity: " + jumpvelocity);
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)){ 
            crouchIsPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchIsPressed = false;            
        }


        if (controller.collissions.above || controller.collissions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collissions.below && !controller.wasCrouchedLastFrame)
        {
            velocity.y = jumpvelocity;
        }

        float targetVelocityX = 0f;
        if (controller.wasCrouchedLastFrame && controller.collissions.below)
        {
            targetVelocityX = input.x * movespeed * croucheSpeedMultiplier;
        }
        else
        {
            targetVelocityX = input.x * movespeed;
        }
        

        //Erlaubt ein momentumbasiertes Bewegunssystem
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collissions.below)?accelerationTimeGrounded:accelerationTimeAirborn);

        velocity.y += gravity * Time.deltaTime;

        //Bewegt den Spieler
        controller.Move(velocity * Time.deltaTime, false, crouchIsPressed);
    }
}
