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

    public void ChangeSprite()
    {
        renderer.sprite = checkPointAktivated;
    }

    
}
