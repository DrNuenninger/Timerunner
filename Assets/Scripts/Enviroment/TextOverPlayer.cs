using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOverPlayer : MonoBehaviour
{
    public GameObject textToDisplay;
    public float displayDuration = 3;
    private bool currentlyDisplayed = false;
    
    void Update()
    {
        if (currentlyDisplayed)
        {
            DisplayTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DisplayText();
        }

    }

    private void DisplayTimer()
    {
        if (displayDuration <= 0)
        {
            StopDisplayText();
        }

        displayDuration -= Time.deltaTime;
    }

    private void DisplayText()
    {
        currentlyDisplayed = true;
        textToDisplay.SetActive(true);
    }

    private void StopDisplayText()
    {
        currentlyDisplayed = false;
        textToDisplay.SetActive(false);
        Destroy(gameObject);
    }
}
