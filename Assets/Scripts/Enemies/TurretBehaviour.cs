using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public GameObject missle;
    private Transform shootingPosition;
    public float DelayBetweenShots = 4f;
    private float DelayToPass = 0f; 

    void Start()
    {
        shootingPosition = GameObject.Find("Barrel_Tip").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (DelayToPass <= 0)
        {
            Instantiate(missle, shootingPosition.position, shootingPosition.rotation);
            DelayToPass = DelayBetweenShots;
        }
        else
        {
            DelayToPass = DelayToPass - Time.deltaTime;
        }
        
    }
}
