using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public float delayBetweenShots = 4f;
    public float activationRange = 5f;
    public float shootingMargin = 10f;
    public GameObject missle;
    public Transform shootingPosition;
    private TurretAim turretAim;
    private Transform player;
    private float DelayToPass = 0.1f; 

    void Start()
    {
        player = GameObject.Find("Player").transform;
        turretAim = gameObject.GetComponent<TurretAim>();
    }

    void Update()
    {
        var distance = (player.position - transform.position).magnitude;

        if (distance <= activationRange)
        {
            turretAim.isActive = true;
        }
        else
        {
            turretAim.isActive = false;
        }
        if (DelayToPass <= 0)
        {   
            if (turretAim.angleToPlayer <= shootingMargin)
            {
                Instantiate(missle, shootingPosition.position, shootingPosition.rotation);
                DelayToPass = delayBetweenShots;
            }
        }
        else
        {
            DelayToPass = DelayToPass - Time.deltaTime;
        }
        
    }
}
