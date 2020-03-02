using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    //Will most likely be dropped when we add animations as there is an in unity tool for those
    private new SpriteRenderer renderer;
    public Sprite standingSprite;
    public Sprite crouchingSprite;
    public Sprite wallSlidingSprite;
    public Sprite crouchSlidingSprite;

    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipSpriteX(bool flipX)
    {
        renderer.flipX = flipX;
    }

    public void UpdateSprite(Sprite newSprite)
    {
        renderer.sprite = newSprite;
    }
}
