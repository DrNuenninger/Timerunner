using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToCheckPoints : MonoBehaviour
{
    public Transform originalSpawnPoint;
    private Transform currentSpawnPoint;
    private CheckpointBehaviour checkpointBehaviour;
    private Level_Information information;

    void Start()
    {
        currentSpawnPoint = originalSpawnPoint;
        information = GameObject.Find("Level_Information").GetComponent<Level_Information>();
        if (information == null) Debug.LogError("Lever_Information not set!");
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
                checkpointBehaviour.ChangeSprite();

                information.SaveOrbs();

                FindObjectOfType<SoundManager>().Play("CheckpointAktivated");
                print("Setting Spawnpoint to : " + col.name);
            }
        }
    }


    public Transform GetCurrentSpawnpoint()
    {
        return currentSpawnPoint;
    }
}
