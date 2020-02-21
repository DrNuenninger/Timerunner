using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float hSpeed = 0f;
    private float vSpeed = 0f;
    private float acceleration = 10f;
    private Rigidbody2D component;



    void Start()
    {
        component = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        
        //Horizontal Movement
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (FastApproximately(hSpeed, 0, 0.0001f)) hSpeed = 0;
            if (hSpeed > 0) hSpeed -= 0.05f;
            if (hSpeed < 0) hSpeed += 0.05f;
        }
        

        if (Input.GetKey(KeyCode.A))
        {
            hSpeed -= 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            hSpeed += 0.1f;
        }

        //Limits hSpeed & vSpeed each frame
        if (hSpeed > 5) hSpeed = 5;
        if (hSpeed < -5) hSpeed = -5;

        Debug.Log("hSpeed = " + hSpeed);

        //Vertical Movement

        //Falls if no ground below (TODO)
        if (vSpeed > 0)
        {
            vSpeed -= 0.1f;
        }

        if (vSpeed < 0) vSpeed = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            vSpeed = 10f;
        }



        component.velocity = new Vector2(hSpeed, vSpeed);

    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
