using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            switchToTime();
        }
    }
    void switchToTime()
    {
        FindObjectOfType<SoundManager>().Play("TimeSwitch");
        presentOrPast = !presentOrPast;
        level_present.SetActive(presentOrPast);
        level_past.SetActive(!presentOrPast);
        sendSwitchAnalytics();
    }
    void sendSwitchAnalytics()
    {

        float posx = GameObject.Find("Player").transform.position.x;
        //float posx = this.transform.position.x;
        //float posy = this.transform.position.y;
        float posy = GameObject.Find("Player").transform.position.y;
        UnityEngine.Analytics.Analytics.CustomEvent("switch_Time", new Dictionary<string, object>
        {
            {"PosX", posx },
            {"PosY", posy },
            {"Reason", presentOrPast ? "presentToPast" : "PastToPresent" },
            {"Level", SceneManager.GetActiveScene().name }

        });
    }
}
