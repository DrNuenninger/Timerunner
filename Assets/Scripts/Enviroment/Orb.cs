using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public bool isCollected = false;
    public bool isSaved = false;
    private int index;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Collect()
    {
        transform.parent.gameObject.GetComponent<Level_Information>().CollectOrb(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

 

    public int GetIndex()
    {
        return index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if(collision.tag == "Player")
        {
            Collect();
        }
    }
}
