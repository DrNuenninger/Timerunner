using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneLogic : MonoBehaviour
{

    private BoxCollider2D killZoneCollider;
    private Vector2 killZoneAreaSize;

    private KillzoneArea killZoneArea;

    struct KillzoneArea
    {
        public readonly Vector2 center;

        public KillzoneArea(Bounds killZoneBounds, Vector2 size)
        {
            center = killZoneBounds.center;
        }
    }

    void Start()
    {
        killZoneCollider = transform.GetComponent<BoxCollider2D>();
        killZoneAreaSize = killZoneCollider.size;

        killZoneArea = new KillzoneArea(killZoneCollider.bounds, killZoneAreaSize);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(killZoneArea.center, killZoneAreaSize);
    }
}
