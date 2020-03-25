using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    public float minLeftAngle = 0f;
    public float maxLeftAngle = 180f;
    public float minRightAngle = 0f;
    public float maxRightAngle = 180f;
    public float aimSpeed = 0.8f;

    public Transform rotationAnchorpoint;
    public Transform barrelPosition0;
    public Transform barrel_Tip;
    public Transform barrel;

    public bool isActive = false;
    public float angleToPlayer = 0.0f;

    private Transform player;
    private SpriteRenderer barrelSprite;
    private Vector3 barrelDirection;
    private float angle = 0.0f;

    void Start()
    {
        player = GameObject.Find("Player").transform;

        barrelDirection = rotationAnchorpoint.position - barrelPosition0.position;
        barrelSprite= barrel.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        angle = Vector2.Angle(barrelDirection, rotationAnchorpoint.position - player.position);
        float direction = Vector3.Cross(barrelDirection, rotationAnchorpoint.position - player.position).normalized.z;
        angle = angle * direction;

        angleToPlayer = Vector2.Angle(rotationAnchorpoint.position - barrel_Tip.position, rotationAnchorpoint.position - player.position);

        barrelSprite.flipX = rotationAnchorpoint.localRotation.z > 0;

        if (direction == -1)
        {
            angle = Mathf.Clamp(angle, maxRightAngle * -1,minRightAngle * -1);     
        }
        else
        {
            angle = Mathf.Clamp(angle, minLeftAngle, maxLeftAngle); 
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotationAnchorpoint.localRotation = Quaternion.RotateTowards(rotationAnchorpoint.localRotation, rotation, aimSpeed);       
    }
}

