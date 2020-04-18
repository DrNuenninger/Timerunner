using UnityEngine;

public class goombaAi : RayCastController
{
    public float speed = 0;
    private bool movingRight = true;
    private Vector3 velocity = Vector3.zero;

    public override void Start()
    {

        base.Start();
        velocity.x = speed;
    }

    void Update()
    {
        UpdateRayCastOrigins();
        CheckVerticalCollision();
        CheckHorizontalCollisionAndMove();
    }

    private void CheckVerticalCollision() {
        Vector2 rayOrigin = (movingRight) ? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.5f, collissionMask);

        Debug.DrawRay(rayOrigin, Vector2.down, Color.red);

        if (!hit)
        {   
            TurnAround();
        }
    }

    private void CheckHorizontalCollisionAndMove()
    {
        Vector2 rayOrigin = (movingRight) ? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft;
        Vector2 direction = (movingRight) ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, velocity.x * Time.deltaTime, collissionMask);

        direction.x = velocity.x * Time.deltaTime;
        Debug.DrawRay(rayOrigin, direction, Color.red);

        if (hit)
        {
            TurnAround();
        }
        else
        {
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    private void TurnAround()
    {
        if (movingRight)
        {
            velocity.x = -speed;
            movingRight = false;
        }
        else
        {
            velocity.x = speed;
            movingRight = true;
        }

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
