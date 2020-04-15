using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCrystal : MonoBehaviour
{
    public SwitchTime switchTime;
    public HintLevel hintLevel;

    void Start()
    {
        switchTime.timeSwitchPossible = false;
        hintLevel.hintLevelPossible = false;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Destroy(gameObject);
            switchTime.timeSwitchPossible = true;
            hintLevel.hintLevelPossible = true;
        }
    }
}

