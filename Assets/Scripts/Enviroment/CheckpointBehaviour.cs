using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    public Sprite checkPointAktivated;
    public bool isAktivated = false;
    private new SpriteRenderer renderer;
    

    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !isAktivated)
        {
            renderer.sprite = checkPointAktivated;
        }
    }
}
