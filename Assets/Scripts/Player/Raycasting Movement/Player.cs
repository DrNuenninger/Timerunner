using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private float movespeed = 6f;
    private float jumpvelocity = 8f;
    private Vector3 velocity;
    public float gravity = -20f;

    public Controller2D controller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collissions.above || controller.collissions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collissions.below)
        {
            velocity.y = jumpvelocity;
        }

        velocity.x = input.x * movespeed;
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
