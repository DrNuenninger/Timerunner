using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goombaAi : RayCastController
{
    public float speed = 0;
    bool movingRight = true;
  
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        UpdateRayCastOrigins();

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = rayCastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 2f, collissionMask);
            Debug.DrawRay(rayOrigin, Vector2.down, Color.red);

            if (!hit)
            {
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
                break;
            }
       
        }
        
    }
}
