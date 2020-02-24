using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour
{

    public const float skinwidth = 0.015f;
    public LayerMask collissionMask;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public BoxCollider2D collider;
    public RayCastOrigins rayCastOrigins;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    // Start is called before the first frame update
    public virtual void Start()
    {
        CalculateRaySpacing(false);
    }

    public struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

  

    public void UpdateRayCastOrigins(bool isCrouched = false)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);
        
        if (!isCrouched)
        {            
            rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        else
        {
            print("Crouching Rays");
            rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.center.y);
            rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.center.y);
            CalculateRaySpacing(isCrouched);
        }


        rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

    }

    public void CalculateRaySpacing(bool isCrouched)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);


        if (!isCrouched)
        {
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
        else
        {
            horizontalRaySpacing = (bounds.size.y/2) / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
    }
}
