﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Controller2D target;
    public Vector2 focusAreaSize;

    private FocusArea focusArea;

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        public float left, right;
        public float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;
            velocity = Vector2.zero;
            center = new Vector2((left+right)/2, (top+bottom)/2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0f;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0f;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            bottom += shiftY;
            top += shiftY;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(target.collider.bounds);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,0f,0f,0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
}
