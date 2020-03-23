using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    public float minLeftAngle = 0f;
    public float maxLeftAngle = 180f;
    public float minRightAngle = 0f;
    public float maxRightAngle = 180f;
    private Transform rotationAnchorpoint;
    private Transform barrelPosition0;
    private Transform player;
    private Vector3 barrelDirection;
    private Vector3 barrelToAnchorpointDirection;
    private Quaternion q;
    private float angle = 0.0f;
    private float angle2 = 0.0f;
    private Transform Barrel_Tip;
    void Start()
    {
        rotationAnchorpoint = GameObject.Find("Rotation_anchorpoint").transform;
        player = GameObject.Find("Player").transform;
        barrelPosition0 = GameObject.Find("Barrel_position0").transform;
        Barrel_Tip = GameObject.Find("Barrel_Tip").transform;

        barrelDirection = rotationAnchorpoint.position - barrelPosition0.position;
        barrelToAnchorpointDirection = (transform.position - rotationAnchorpoint.position);
    }

    void Update()
    {
        angle = Vector2.Angle(barrelDirection, rotationAnchorpoint.position - player.position);
        float direction = Vector3.Cross(barrelDirection, rotationAnchorpoint.position - player.position).normalized.z * -1;
        angle = angle * direction;

        if (direction == 1)
        {
            angle = Mathf.Clamp(angle, minRightAngle, maxRightAngle);
        }
        else
        {
            angle = Mathf.Clamp(angle, maxLeftAngle * -1, minLeftAngle *-1);
        }

        q = Quaternion.AngleAxis(angle, Vector3.back);
      
        transform.position = rotationAnchorpoint.position + q * barrelToAnchorpointDirection;
        transform.rotation = q;
    }

    void OnGUI()
    {
        var difference = q.eulerAngles - transform.rotation.eulerAngles;
        GUI.Label(new Rect(10, 10, 100, 20), "Angle: " + angle);

        GUI.Label(new Rect(10, 30, 200, 20), "Quaternion: " + q.eulerAngles);
        GUI.Label(new Rect(10, 50, 200, 20), "Transform: " + transform.rotation.eulerAngles);
        GUI.Label(new Rect(10, 70, 200, 20), "Difference: " + difference);
        GUI.Label(new Rect(10, 90, 200, 20), "Quaternion: " + q);
        GUI.Label(new Rect(10, 110, 200, 20), "CurrentAngle: " + angle2);

    }
}

