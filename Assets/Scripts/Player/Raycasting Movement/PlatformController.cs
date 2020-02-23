using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RayCastController
{
    public Vector3 move;
    public LayerMask passengerMask;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCastOrigins();
        Vector3 velocity = move * Time.deltaTime;
        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    void MovePassengers(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Vertical moving Platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinwidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushY = velocity.y - (hit.distance - skinwidth) * directionY;
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }

                }
            }
        }

        //Horizontially Moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinwidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushY = 0;
                        float pushX = velocity.x - (hit.distance - skinwidth) * directionX;
                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }

                }
            }
        }
    }
}
