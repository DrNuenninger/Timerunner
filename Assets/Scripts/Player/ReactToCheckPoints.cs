using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToCheckPoints : MonoBehaviour
{
    public Transform originalSpawnPoint;
    private Transform currentSpawnPoint;
    private CheckpointBehaviour checkpointBehaviour;

    void Start()
    {
        currentSpawnPoint = originalSpawnPoint;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Checkpoint")
        {
           
            checkpointBehaviour = col.gameObject.GetComponent<CheckpointBehaviour>();
          
            if (!checkpointBehaviour.isAktivated)
            {
                currentSpawnPoint = col.transform;
                checkpointBehaviour.isAktivated = true;
                print("Setting Spawnpoint to : " + col.name);
            }
        }
    }


    public Transform GetCurrentSpawnpoint()
    {
        return currentSpawnPoint;
    }
}
