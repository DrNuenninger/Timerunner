using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float hSpeed = 0f;
    public float jumpSpeed = 10f;
    private float acceleration = 10f;
    private Rigidbody2D component;

    public Transform ground;
    public float groundCheckRadius;
    public LayerMask groundLayer;



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

        //Limits hSpeed each frame
        if (hSpeed > 5) hSpeed = 5;
        if (hSpeed < -5) hSpeed = -5;

        Debug.Log("hSpeed = " + hSpeed);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            component.velocity = new Vector2(hSpeed, jumpSpeed);
            
        }



        component.velocity = new Vector2(hSpeed, component.velocity.y);

    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
