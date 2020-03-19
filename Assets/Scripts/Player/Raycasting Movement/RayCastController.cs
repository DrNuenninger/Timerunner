using System.Collections;
using System.Collections.Generic;
using System.Numerics;
//using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour
{

    public const float skinwidth = 0.015f;
    public LayerMask collissionMask;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector]
    public BoxCollider2D collider;
    public RayCastOrigins rayCastOrigins;
    public SquashingRayCastOrigins squashingRayCastOrigins;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    [HideInInspector]
    public float squashingHorizontalRaySpacing;
    [HideInInspector]
    public float squashingVerticalRaySpacing;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
        UpdateRayCastOrigins();
    }

    public struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct SquashingRayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    //Aktuallisiert die Position der Ecken des Spielers
    public void UpdateRayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);
        
        
        rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        squashingRayCastOrigins.topLeft = new Vector2(bounds.min.x + bounds.size.x/3, bounds.max.y - bounds.size.y/4);
        squashingRayCastOrigins.topRight = new Vector2(bounds.max.x - bounds.size.x / 3, bounds.max.y - bounds.size.y / 4);
        squashingRayCastOrigins.bottomLeft = new Vector2(bounds.min.x + bounds.size.x / 3, bounds.min.y + bounds.size.y / 4);
        squashingRayCastOrigins.bottomRight = new Vector2(bounds.max.x - bounds.size.x / 3, bounds.min.y + bounds.size.y / 4);

    }

    


    //Berechnet den Abstand der einzelnen Rays, nimmt in beachtung ob der Spieler am ducken ist
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinwidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

        squashingHorizontalRaySpacing = (bounds.size.y / 2) / (horizontalRayCount - 1);
        squashingVerticalRaySpacing = (bounds.size.x / 3) / (verticalRayCount - 1);

    }
}
