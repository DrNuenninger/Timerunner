using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private new SpriteRenderer renderer;
    public Sprite standingSprite;
    public Sprite crouchingSprite;

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
