using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    private BoxCollider2D collider;
    private RayCastOrigins rayCastOrigins;
    const float skinwidth = 0.015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;


    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    void UpdateRayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);

        rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y); 
        rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCastOrigins();
        CalculateRaySpacing();

        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(rayCastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2,Color.red);
        }
    }
}
