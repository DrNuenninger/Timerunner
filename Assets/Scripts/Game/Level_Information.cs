using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Information : MonoBehaviour
{
    public string sceneName;
    private List<Transform> orbs;

    private int orbAmount = 0;
    private int orbsCollected = 0;
    private int orbsCollectedSaved = 0;

    void Start()
    {

        orbs = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.tag == "Orb")
            {
                orbs.Add(child);

                child.gameObject.AddComponent<Orb>();
                Orb script = child.gameObject.GetComponent<Orb>();
                script.SetIndex(orbAmount);
                orbAmount++;
            }
        }
    }

    public void CollectOrb(int index)
    {
        foreach(Transform orb in orbs)
        {
            if (orb.gameObject.GetComponent<Orb>().GetIndex() == index)
            {
                orbsCollected++;
                //print("Collecting Orb: " + orb.gameObject.GetComponent<Orb>().GetIndex());
                orb.gameObject.GetComponent<Orb>().isCollected = true;
                orb.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                orb.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void SaveOrbs()
    {
        foreach (Transform orb in orbs)
        {
            if(orb.gameObject.GetComponent<Orb>().isCollected && !orb.gameObject.GetComponent<Orb>().isSaved)
            {
                //print("Saving Orb: " + orb.gameObject.GetComponent<Orb>().GetIndex());
                orb.gameObject.GetComponent<Orb>().isSaved = true;
                orbsCollectedSaved++;
            }
                
        }
    }

    public void LoadPersistenceOnRespawn()
    {        
        orbsCollected = 0;
        foreach (Transform orb in orbs)
        {
            if (orb.gameObject.GetComponent<Orb>().isCollected && !orb.gameObject.GetComponent<Orb>().isSaved)
            {
                //print("Aktivateing Orb: " + orb.gameObject.GetComponent<Orb>().GetIndex());
                orb.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                orb.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                orb.gameObject.GetComponent<Orb>().isCollected = false;
                
            }

            if (orb.gameObject.GetComponent<Orb>().isCollected && orb.gameObject.GetComponent<Orb>().isSaved)
            {                
                orbsCollected++;
            }

        }
    }
   
}
