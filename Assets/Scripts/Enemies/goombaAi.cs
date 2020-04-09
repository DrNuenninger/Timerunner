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
        
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
        UpdateRayCastOrigins();
        //CalculateRaySpacing();
        
            Vector2 rayOrigin = (movingRight)? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft;
            //rayOrigin += Vector2.right *velocity.x;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 2f, collissionMask);

            Debug.DrawRay(rayOrigin, Vector2.down, Color.red);

            if (!hit)
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
            }           
       
        
        print(movingRight);
        transform.Translate(velocity * Time.deltaTime);

    }
}
