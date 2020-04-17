using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public float delayBetweenShots = 4f;
    public float activationRange = 5f;
    public float shootingMargin = 10f;
    public GameObject missle;
    private GameObject level;
    public Transform shootingPosition;
    private TurretAim turretAim;
    private Transform player;
    private float DelayToPass = 0.1f;

    private bool notAffectedByTime = false;
    private bool isBasedInPast;


    void Start()
    {
        player = GameObject.Find("Player").transform;
        level = GetLevelObject();
        turretAim = gameObject.GetComponent<TurretAim>();
        isBasedInPast = DeterminIfTurretIsInPast();

    }

    private bool DeterminIfTurretIsInPast()
    {
        Transform t = transform;
        while (t.parent != null)
        {
            if (t.parent.name == "Level_Past")
            {
                return true;
            }
            if (t.parent.name == "Level_Present")
            {
                return false;
            }
            t = t.parent.transform;
        }
        Debug.LogError("No Time Level Object Found!");
        notAffectedByTime = true;
        return false;
    }

    private GameObject GetLevelObject()
    {
        Transform t = transform;
        while (t.parent != null)
        {
            if (t.parent.name == "Level")
            {
                return t.parent.gameObject;
            }            
            t = t.parent.transform;
        }
        Debug.LogError("No Level Object Found!");
        return null;
    }

    void Update()
    {
        if (level.GetComponent<SwitchTime>().switchingToPresent != isBasedInPast || notAffectedByTime)
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

            DelayToPass = DelayToPass - Time.deltaTime;
            if (DelayToPass <= 0 && distance <= activationRange)
            {
                if (turretAim.angleToPlayer <= shootingMargin)
                {
                    Instantiate(missle, shootingPosition.position, shootingPosition.rotation);
                    DelayToPass = delayBetweenShots;
                }
            }
        }

    }
}
