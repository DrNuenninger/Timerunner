using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlider : MonoBehaviour
{
    public Collider2D player;
    public bool lockX = false;
    public bool lockY = false;
    public int zoom = -10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float camX = (lockX == true) ? transform.position.x : player.bounds.center.x;
        float camY = (lockY == true) ? transform.position.y : player.bounds.center.y;
        
            transform.position = new Vector3(camX, camY, zoom);
       
    }
}
