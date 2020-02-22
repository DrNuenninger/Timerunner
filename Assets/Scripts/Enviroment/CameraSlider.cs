using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlider : MonoBehaviour
{
    public Rigidbody2D player;
    public bool lockX = false;
    public bool lockY = false;
    public int zoom = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!lockY)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, zoom);
        }
        if (!lockX)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, zoom);
        }
    }
}
