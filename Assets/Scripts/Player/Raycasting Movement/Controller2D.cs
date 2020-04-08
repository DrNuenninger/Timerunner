using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Analytics;
using System.Collections.Generic;


public class Controller2D : RayCastController
{
    public float maxClimbAngle = 60f;
    public float maxDecentAngle = 60f;
    private float crouchHeight;
    public bool wasCrouchedLastFrame = false;
    private bool crouchColliderIsCrouched = false;
    private bool playerAbleToStandUp = false;
    public bool isDead = false;
    public bool isSplashed = false;
    public bool isLevelBoundry = false;
    private Player player;


    private SpriteManager spriteManager;

    public CollisionInfo collissions;
   
    [HideInInspector]
    public Vector2 playerInput;

    Vector3 oldColliderSize;
    Vector3 oldColliderOffset;

    public override void Start()
    {
        
        base.Start();
        crouchHeight = collider.size.y / 2;
        oldColliderSize = collider.size;
        oldColliderOffset = collider.offset;
        collissions.faceDirection = 1;
        spriteManager = this.GetComponent<SpriteManager>();
        player = this.GetComponent<Player>();
    }

    public void deathByMissile()
    {
        sendDeathAnalytics("Missile");
    }

    public void deathByMine()
    {
        sendDeathAnalytics("Mine");
    }

    void ResetFallingThroughPlatform()
    {
        collissions.fallingThorughPlatform = false;
    }


    public SpriteManager getSpriteManager()
    {
        return spriteManager;
    }

    public void AdjustAnimations()
    {

    }
    

    //Informationen zu der Kollision des Spielers mit der Umgebung (Datenstrucktur)
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDirection;
        public bool fallingThorughPlatform;

        
        

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }
    }

    void UnCrouch()
    {
        collider.size = oldColliderSize;
        collider.offset = oldColliderOffset;
    }
    
    void Crouch()
    {
        Vector3 currentSize = oldColliderSize;
        Vector3 currentOffset = oldColliderOffset;
        currentSize.y -= crouchHeight;
        currentOffset.y -= crouchHeight / 2;

        collider.size = currentSize;
        collider.offset = currentOffset;
    }

    //Überprüft ob "ducken" gedrückt wird
    void CalculatCrouching(bool isCrouched, ref Vector3 velocity)
    {
        //print("Was Crouched: " + wasCrouchedLastFrame + " Is Crouched: " + isCrouched);
        UpdateRayConfiguration();
        if (isCrouched)
        {
            if (!crouchColliderIsCrouched)
            {
                Crouch();
                crouchColliderIsCrouched = true;
            }            
            wasCrouchedLastFrame = true;
        }
       
        if(!isCrouched && wasCrouchedLastFrame)
        {           
            if (playerAbleToStandUp)
            {
                UnCrouch();
                crouchColliderIsCrouched = false;
                wasCrouchedLastFrame = false;
            }            
        }
        
    }

    void UpdateRayConfiguration()
    {
        CalculateRaySpacing();
        UpdateRayCastOrigins();
    }


    //Überprüft ob, wenn der Spieler aufsteht, eine Wand/Decke in ihm drin sein würde
    bool CheckIfPlayAbleToStand(ref Vector3 velocity)
    {        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = rayCastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * 1, collider.size.y, collissionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * 1, Color.blue);

            if (hit)
            {
                return false;
            }
        }
        return true;
    }

    void sendDeathAnalytics(string variant)
    {
        float posx = this.transform.position.x;
        float posy = this.transform.position.y;
        string reason = variant;
        UnityEngine.Analytics.Analytics.CustomEvent("player_death", new Dictionary<string, object>
        {
            {"PosX", posx },
            {"PosY", posy },
            {"Reason", reason },
            {"Level", SceneManager.GetActiveScene().name }

        });
    }

    //Behandelt horiziontale Bewegung
    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collissions.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinwidth;

        if (Mathf.Abs(velocity.x) < skinwidth)
        {
            rayLength = 2 * skinwidth;
        }
        
        
        //Strahlt Rays von dem Character nach links oder rechts aus
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collissionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if(hit.collider.tag == "lvlBoundry")
                {
                    isLevelBoundry = true;
                }
                else
                {
                    isLevelBoundry = false;
                }

                if (hit.distance == 0 && hit.collider.tag != "PassablePlatform")
                {
                    for (int e = 0; e < horizontalRayCount; e++)
                    {
                        //print("FaceDirection = " + collissions.faceDirection);
                        Vector2 rayOrigin2 = (directionX == -1) ? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft ;
                        rayOrigin2 += Vector2.up * (horizontalRaySpacing * i);
                        RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin2, Vector2.right * -directionX, rayLength, collissionMask);

                        Debug.DrawRay(rayOrigin2, Vector2.right * -directionX, Color.magenta);

                        if (hit2)
                        { 
                            if(hit2.distance == 0)
                            {
                                //Player is Squashed
                                print("Kill Player");
                                isDead = true;
                                sendDeathAnalytics("Crushed");
                                isSplashed = true;
                                break;                                
                            }
                        }
                    }

                    velocity.x = -directionX * skinwidth * 9;
                    continue;
                }

                if (hit.collider.tag == "PassablePlatform")
                {
                    continue;
                }

                //Sorgt dafür, dass man durch eine Wand laufen kann, wenn man eigentlich stuck werden würde
                //TODO: Verhalten hinzufügen dafür, wenn man in einer Wand zu tief drin ist
                if (hit.distance == 0)
                {
                    continue;                    
                }

                //Holt den winkel, abhängig von der normalen, auf die man zuläuft
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collissions.descendingSlope)
                    {
                        collissions.descendingSlope = false;
                        velocity = collissions.velocityOld;
                    }
                    float distanceToSlopeStart = 0f;
                    if (slopeAngle != collissions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinwidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collissions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinwidth) * directionX;
                    rayLength = hit.distance;

                    if (collissions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collissions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collissions.left = directionX == -1;
                    collissions.right = directionX == 1;
                }
            }
            else
            {
                isLevelBoundry = false;
            }
        }
    }

    //Anpassung der velocity um eine aufsteigende Rampe zu berücksichtigen
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collissions.below = true;
            collissions.climbingSlope = true;
            collissions.slopeAngle = slopeAngle;
        }
    }

    //Anpassung der velocity um eine absteigende Rampe zu berücksichtigen
    void DecentSlope(ref Vector3 velocity)
    {
        //Strahlt einen Ray nach unten, um den Winkel der absteigenden Rampe zu erhalten
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collissionMask);

        //Wenn absteigende Rampe vorhanden & Bewegung in diese Richtung erfolgt, bewege dich die Rampe herunter
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDecentAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinwidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collissions.slopeAngle = slopeAngle;
                        collissions.descendingSlope = true;
                        collissions.below = true;
                    }
                }
            }
        }

    }

   

    
    //Vertikalle Bewegungsberechnung
    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinwidth;
        //Rays werden nach oben und unten abhängig von der bewegungsrichtung ausgestrahlt
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? rayCastOrigins.bottomLeft:rayCastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up*directionY, rayLength, collissionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
            
            //Wenn ein Strahl ein Ground Objekt trifft bewege den Spieler bis zu dem objekt
            if (hit)
            {
                if(hit.distance == 0 && hit.collider.tag != "PassablePlatform")
                {
                    velocity.y = -directionY * skinwidth;
                    continue;
                }
                if (hit.collider.tag == "PassablePlatform")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collissions.fallingThorughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collissions.fallingThorughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }

                velocity.y = (hit.distance - skinwidth) * directionY;
                rayLength = hit.distance;

                if (collissions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collissions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collissions.below = directionY == -1;
                collissions.above = directionY == 1;
            }
        }

        if (collissions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinwidth;
            Vector2 rayOrigin = ((directionX == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collissionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collissions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinwidth) * directionX;
                    collissions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false, bool isCrouched = false,
        bool wallSliding = false)
    {
        Move(velocity, Vector2.zero, standingOnPlatform, isCrouched, wallSliding);
    }


    //Keine wirkliche Berechnungen, called die Kalkulierenden Funktionen abhängig von der Bewegungsrichung und Transformiert diese mit dem Spieler
    public void Move(Vector3 velocity,Vector2 input, bool standingOnPlatform = false, bool isCrouched = false, bool wallSliding = false, bool isCrouchSliding = false)
    {
        UpdateRayCastOrigins();
        collissions.Reset();
        //CheckIfPlayerSquashed(ref velocity);
        if (wasCrouchedLastFrame && !isCrouched)
        {
            playerAbleToStandUp = CheckIfPlayAbleToStand(ref velocity);
        }
        CalculatCrouching(isCrouched, ref velocity);
        collissions.velocityOld = velocity;



        playerInput = input;

        

        if (velocity.y < 0)
        {           
            DecentSlope(ref velocity); 
        }

        if (velocity.x != 0)
        {
            collissions.faceDirection = (int)Mathf.Sign(velocity.x);
        }
        HorizontalCollisions(ref velocity);
        
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);

        if (standingOnPlatform)
        {
            collissions.below = true;
        }

        ChangeSprites(collissions.faceDirection, wallSliding, isCrouchSliding);
    }

    public void ChangeSprites(int faceDirection, bool isWallSliding = false, bool isCrouchSliding = false)
    {
        if(faceDirection == 1)
        {
            spriteManager.FlipSpriteX(true);
            
        }
        else
        {
            spriteManager.FlipSpriteX(false);
        }

        if (isCrouchSliding)
        {
            spriteManager.UpdateSprite(spriteManager.crouchSlidingSprite);
        }
        else if (wasCrouchedLastFrame)
        {
            spriteManager.UpdateSprite(spriteManager.crouchingSprite);
        }else if (isWallSliding)
        {
            spriteManager.UpdateSprite(spriteManager.wallSlidingSprite);
        }
        else
        {
            spriteManager.UpdateSprite(spriteManager.standingSprite);
        }
    }
}
