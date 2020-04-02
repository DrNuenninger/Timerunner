using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static Load_Manager;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Sprite))]
public class Player : MonoBehaviour
{
    public Animator animator;

    private ReactToCheckPoints checkpoints;
    public GameObject blood;
    private bool bloodIsSpawned = false;
    private bool deathSoundIsPlayed = false;
    

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    private bool crouchIsPressed;
    private bool lockMovement = false;

    private bool playerHasImpacted = false;
    
    public float movespeed = 6f;
    public float sprintSpeedModifier = 2f;
    public float accelerationTimeSprint = 0.4f;
    public float maxBonusSpeed = 5f;
    public float accelerationTimeBonusSpeed = 2f;
    private float bonusSpeedSmoothingLeft;
    private float bonusSpeedSmoothingRight;
    private float bonusSpeedSmoothingStop;

    private float currentBonusSpeed = 0f;
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
    
    private float slideTimer = 0f;
    public bool isCrouchSliding = false;
    public bool crouchSlideSlowdown = false;
    public float maxExtraCrouchSlideSpeed = 12f;
    private float extraCrouchSlideSpeed;
    public float minCrouchSlideExtraSpeedAngle = 20f;
    public float crouchSlideSlowdownTime = 0.5f;
    public float maxCrouchSlideTime = 1f;
   
    public bool initPossibleCrouchSlide;
    public bool playedCrouchSlideAudio = false;
    private Level_Information information;
    



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
        checkpoints = this.GetComponent<ReactToCheckPoints>();
        //Berechnet die Schwerkraft und Sprunggeschwindigkeit
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        information = GameObject.Find("Level_Information").GetComponent<Level_Information>();
        if (information == null) Debug.LogError("Lever_Information not set!");

        print("Gravity: " + gravity + " JumpVelocity: " + maxJumpVelocity);
        
    }

    void Respawn()
    {
        information.LoadPersistenceOnRespawn();
        velocityXSmoothing = new float();
        velocity.x = 0f;
        velocity.y = 0f;
        controller.wasCrouchedLastFrame = false;
        isCrouchSliding = false;
        crouchIsPressed = false;
        currentSprintSpeed = 0f;
        transform.position = checkpoints.GetCurrentSpawnpoint().position;
        controller.isDead = false;
        controller.isSplashed = false;
        bloodIsSpawned = false;
        deathSoundIsPlayed = false;
    }
 
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            controller.isDead = true;
        }
        if (controller.isDead)
        {
            if (!deathSoundIsPlayed)
            {
                if (controller.isSplashed)
                {
                    FindObjectOfType<SoundManager>().Play("PlayerCrushed");
                }
                else
                {
                    FindObjectOfType<SoundManager>().Play("PlayerDeath");
                }
                
                deathSoundIsPlayed = true;
            }
            if (controller.isSplashed && !bloodIsSpawned)
            {
                animator.SetBool("isSplashed", controller.isSplashed);
                Instantiate(blood, transform.position, Quaternion.identity);
                bloodIsSpawned = true;
            }            
            animator.SetBool("isDead", controller.isDead);
            lockMovement = true;
        }
        else
        {
            animator.SetBool("isDead", controller.isDead);
            animator.SetBool("isSplashed", controller.isSplashed);
            lockMovement = false;
        }

        if (lockMovement)
        {
            if (!controller.isSplashed)
            {
                controller.Move(Vector3.down * 5 * Time.deltaTime);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
            return;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirectionX = (controller.collissions.left) ? -1 : 1;
        float targetSprintSpeed = (movespeed * sprintSpeedModifier) - movespeed;
        float targetVelocityX;
        float localCrouchSpeedMultiplier;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Load_Manager.load_Manager.escape_Action();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Mathf.Abs(velocity.x) > Mathf.Abs(1 * movespeed * crouchSpeedMultiplier))
        {
            initPossibleCrouchSlide = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            initPossibleCrouchSlide = false;
        }

        if (controller.wasCrouchedLastFrame /* && controller.collissions.below*/ && !controller.collissions.left && !controller.collissions.right)
        {
            //If Player pressed Ctrl AND is moving faster than crouchspeed => Start sliding
            localCrouchSpeedMultiplier = 0.6f;

            
            if (initPossibleCrouchSlide && !crouchSlideSlowdown)
            {
                slideTimer += Time.deltaTime;
                isCrouchSliding = true;
                //print(playedCrouchSlideAudio);
                if (!playedCrouchSlideAudio)
                {
                    //Need to find better sound
                    //FindObjectOfType<SoundManager>().Play("PlayerCrouchSlide");
                    playedCrouchSlideAudio = true;
                }
                
                localCrouchSpeedMultiplier = 1f;
                if (!controller.collissions.below)
                {
                    slideTimer = 0f;
                }
                if (controller.collissions.descendingSlope && controller.collissions.slopeAngle >= minCrouchSlideExtraSpeedAngle)
                {
                    slideTimer = 0f;
                    if(extraCrouchSlideSpeed < maxExtraCrouchSlideSpeed)
                    {
                        extraCrouchSlideSpeed = (Mathf.Sign(input.x) == 1)? maxExtraCrouchSlideSpeed : -maxExtraCrouchSlideSpeed;
                    }
                }
                else
                {
                    extraCrouchSlideSpeed = 0f;
                }
                //print("CrouchSpeedMultiplier = " + localCrouchSpeedMultiplier);
                if (slideTimer >= maxCrouchSlideTime)
                {
                    crouchSlideSlowdown = true;
                    initPossibleCrouchSlide = false;
                    slideTimer = 0f;
                }
            }
            else
            {
                isCrouchSliding = false;
                playedCrouchSlideAudio = false;
            }

            if (crouchSlideSlowdown)
            {
                currentSprintSpeed = 0f;
                extraCrouchSlideSpeed = 0f;
                localCrouchSpeedMultiplier = 0.6f;
                crouchSlideSlowdown = false;
                isCrouchSliding = false;
                playedCrouchSlideAudio = false;
            }

        }
        else
        {
            slideTimer = 0f;
            isCrouchSliding = false;
            playedCrouchSlideAudio = false;
            localCrouchSpeedMultiplier = 1f;
            extraCrouchSlideSpeed = 0f;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            //Wenn der Spieler rennt, sich aber duckt, setze den Sprintspeed wieder richtung 0
            if (controller.wasCrouchedLastFrame && !isCrouchSliding)
            {
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, 0f, ref speedSmoothing,
                    accelerationTimeSprint / 2);

                currentBonusSpeed = Mathf.SmoothDamp(currentBonusSpeed, 0f, ref bonusSpeedSmoothingStop,
                    0.1f);
            }
            else if ( input.x > 0 && !isCrouchSliding)
            {
                bonusSpeedSmoothingLeft = new float();
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, targetSprintSpeed, ref speedSmoothing,
                    accelerationTimeSprint);

                currentBonusSpeed = Mathf.SmoothDamp(currentBonusSpeed, maxBonusSpeed, ref bonusSpeedSmoothingRight,
                    accelerationTimeBonusSpeed);
            }
            else if ( input.x < 0 && !isCrouchSliding)
            {
                bonusSpeedSmoothingRight = new float();
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, -targetSprintSpeed, ref speedSmoothing,
                    accelerationTimeSprint);

                currentBonusSpeed = Mathf.SmoothDamp(currentBonusSpeed, -maxBonusSpeed, ref bonusSpeedSmoothingLeft,
                    accelerationTimeBonusSpeed);
            }
            else if (input.x == 0)
            {
                currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, 0f, ref speedSmoothing,
                    accelerationTimeSprint / 2);

                currentBonusSpeed = Mathf.SmoothDamp(currentBonusSpeed, 0f, ref bonusSpeedSmoothingStop,
                    0.1f);
            }
            else if ((!controller.collissions.below && Mathf.Sign(input.x) == Mathf.Sign(currentSprintSpeed)) || isCrouchSliding)
            {
                //Intended to be empty
            }
            
        }
        else
        {
            currentSprintSpeed = Mathf.SmoothDamp(currentSprintSpeed, 0f, ref speedSmoothing,
                accelerationTimeSprint / 2);

            currentBonusSpeed = Mathf.SmoothDamp(currentBonusSpeed, 0f, ref bonusSpeedSmoothingStop,
                0.1f);
        }
        targetVelocityX = (input.x * movespeed + currentSprintSpeed) * localCrouchSpeedMultiplier + extraCrouchSlideSpeed + currentBonusSpeed;
        //Erlaubt ein momentumbasiertes Bewegunssystem
        if ((controller.collissions.left && input.x < 0) || (controller.collissions.right && input.x > 0))
        {
            velocity.x = 0f;
            currentSprintSpeed = 0f;
            currentBonusSpeed = 0f;
        }
        else
        { 
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                    (controller.collissions.below) ? accelerationTimeGrounded : accelerationTimeAirborn);
        }

        //print("Current Vec: " + velocity.x + " Target Vec: "+ targetVelocityX + " SprintSpeed: " + currentSprintSpeed);
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
            //print("Set Iscoruched");
            crouchIsPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            //print("Unset isCrocued");
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
                FindObjectOfType<SoundManager>().Play("PlayerJump");
            }

            if (controller.collissions.below)
            {
                if (controller.wasCrouchedLastFrame)
                {
                    crouchIsPressed = false;
                }
                velocity.y = maxJumpVelocity;
                FindObjectOfType<SoundManager>().Play("PlayerJump");
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
        controller.Move(velocity * Time.deltaTime, input, false, crouchIsPressed, wallSliding, isCrouchSliding);
        animator.SetFloat("moveSpeed", Mathf.Abs((velocity.x / (movespeed*sprintSpeedModifier))*2));
        animator.SetBool("isCrouched", controller.wasCrouchedLastFrame);
        animator.SetBool("isWallsliding", wallSliding);
        animator.SetBool("isSliding", isCrouchSliding);

        //Player Impact Audio when Player lands on floor
        PlayImpactAudio(wallSliding);




        if (controller.collissions.above || controller.collissions.below)
        {
            velocity.y = 0;
        }

    }


    private float minAirTime = 0.2f;
    private float airTime = 0f;
    private bool airTimeHasPassed = false;
    void PlayImpactAudio(bool isWallSliding)
    {
        if (!playerHasImpacted && !((controller.collissions.below || controller.collissions.descendingSlope || controller.collissions.climbingSlope) || isWallSliding))
        {
            airTime += Time.deltaTime;
            if (airTime >= minAirTime)
            {
                airTimeHasPassed = true;
                airTime = 0f;
            }
        }
        else
        {
            airTime = 0f;
        }
        

        if (airTimeHasPassed && ((controller.collissions.below || controller.collissions.descendingSlope || controller.collissions.climbingSlope) || isWallSliding))
        {
            if (!playerHasImpacted)
            {
                FindObjectOfType<SoundManager>().Play("PlayerImpact");
                playerHasImpacted = true;
                airTimeHasPassed = false;
            }
        }
        else
        {
            playerHasImpacted = false;
        }

    }


}
