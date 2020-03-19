using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehavior : MonoBehaviour
{
    Load_Manager loadManagerController;

    void Start()
    {
        loadManagerController = GameObject.Find("LoadManagerController").GetComponent<Load_Manager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            loadManagerController.load_LevelSelect();
        }
    }
}
