using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTime : MonoBehaviour
{
    // Start is called before the first frame update
    public bool presentOrPast = true;
    GameObject level_past;
    GameObject level_present;
    void Start()
    {
        level_past = gameObject.transform.GetChild(0).gameObject;
        level_present = gameObject.transform.GetChild(1).gameObject;
        level_past.SetActive(!presentOrPast);
        level_present.SetActive(presentOrPast);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            switchToTime();
        }
    }
    void switchToTime()
    {
        presentOrPast = !presentOrPast;
        level_present.SetActive(presentOrPast);
        level_past.SetActive(!presentOrPast);
    }
}
