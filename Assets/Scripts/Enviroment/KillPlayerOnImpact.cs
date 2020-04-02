using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnImpact : MonoBehaviour
{
    Controller2D controller2DPlayer;

    void Start()
    {
        controller2DPlayer = GameObject.Find("Player").GetComponent<Controller2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (this.tag == "BarbedWire") {
                controller2DPlayer.isSplashed = true; }
            controller2DPlayer.isDead = true;
        }
    }

   
}
