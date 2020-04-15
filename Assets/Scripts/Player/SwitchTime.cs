using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SwitchTime : MonoBehaviour
{
    // Start is called before the first frame update
    public bool startInPast = false;
    //Bools sollten nicht entweder oder fragen sein!!!
    public bool switchingToPresent;
    public bool timeSwitchPossible = true;
    GameObject level_past;
    GameObject level_present;
    void Start()
    {
        switchingToPresent = !startInPast;
        level_past = gameObject.transform.Find("Level_Past").gameObject;
        level_present = gameObject.transform.Find("Level_Present").gameObject;
        level_past.SetActive(true);
        level_present.SetActive(true);

        Renderer[] newChildRenderer;
        Collider2D[] newChildColliders;
        Renderer[] oldChildRenderer;
        Collider2D[] oldChildColliders;
        if (startInPast)
        {
            newChildRenderer = level_past.GetComponentsInChildren<Renderer>();
            newChildColliders = level_past.GetComponentsInChildren<Collider2D>();

            oldChildRenderer = level_present.GetComponentsInChildren<Renderer>();
            oldChildColliders = level_present.GetComponentsInChildren<Collider2D>();
        }
        else
        {
            newChildRenderer = level_present.GetComponentsInChildren<Renderer>();
            newChildColliders = level_present.GetComponentsInChildren<Collider2D>();

            oldChildRenderer = level_past.GetComponentsInChildren<Renderer>();
            oldChildColliders = level_past.GetComponentsInChildren<Collider2D>();

        }
        foreach (Renderer r in newChildRenderer) r.enabled = true;
        foreach (Collider2D c in newChildColliders) c.enabled = true;

        foreach (Renderer r in oldChildRenderer) r.enabled = false;
        foreach (Collider2D c in oldChildColliders) c.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && timeSwitchPossible)
        {
            SwitchToTime();
        }
    }
    void SwitchToTime()
    {
        FindObjectOfType<SoundManager>().Play("TimeSwitch");
        switchingToPresent = !switchingToPresent;
        //level_present.SetActive(presentOrPast);
        //level_past.SetActive(!presentOrPast);

        GameObject newTime = (switchingToPresent) ? level_present : level_past;
        GameObject oldTime = (!switchingToPresent) ? level_present : level_past;

        Renderer[] newChildRenderer = newTime.GetComponentsInChildren<Renderer>();
        Collider2D[] newChildColliders = newTime.GetComponentsInChildren<Collider2D>();

        Renderer[] oldChildRenderer = oldTime.GetComponentsInChildren<Renderer>();
        Collider2D[] oldChildColliders = oldTime.GetComponentsInChildren<Collider2D>();

        foreach (Renderer r in newChildRenderer) r.enabled = true;
        foreach (Collider2D c in newChildColliders) c.enabled = true;

        foreach (Renderer r in oldChildRenderer) r.enabled = false;
        foreach (Collider2D c in oldChildColliders) c.enabled = false;

        print("#ColliderNew = " + newChildColliders.Length + " #RendererNew = " + newChildRenderer.Length);

        SendSwitchAnalytics();
    }
    void SendSwitchAnalytics()
    {

        float posx = GameObject.Find("Player").transform.position.x;
        //float posx = this.transform.position.x;
        //float posy = this.transform.position.y;
        float posy = GameObject.Find("Player").transform.position.y;
        UnityEngine.Analytics.Analytics.CustomEvent("switch_Time", new Dictionary<string, object>
        {
            {"PosX", posx },
            {"PosY", posy },
            {"Reason", switchingToPresent ? "presentToPast" : "PastToPresent" },
            {"Level", SceneManager.GetActiveScene().name }

        });
    }
}
