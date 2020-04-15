using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        gameObject.transform.position = player.position;
        gameObject.transform.position += new Vector3(0,2,0);  
    }
}
