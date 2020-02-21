using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    private float hMove = 0f;
    public float runSpeed = 40f;
    public bool jump = false;
    public bool crouchBeingPressed = false;
    public bool wantToStopCrouching = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouchBeingPressed = true;
            wantToStopCrouching = false;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouchBeingPressed = false;
            wantToStopCrouching = true;
        }
    }

    void FixedUpdate() 
    { 
        controller.Move(hMove * Time.fixedDeltaTime, crouchBeingPressed, jump);
        jump = false;
        wantToStopCrouching = false;
       
    }
}
