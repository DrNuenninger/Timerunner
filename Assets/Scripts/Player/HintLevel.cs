using UnityEngine;

public class HintLevel : MonoBehaviour
{
    GameObject hintablePast;
    GameObject hintablePresent;
    SwitchTime switchTime;
    bool presentOrPast = true;
    bool hintInProgress = false;
    float fadeOutFactor;

    void Start()
    {
        switchTime = gameObject.GetComponent<SwitchTime>();
        hintablePast = GameObject.Find("Hint_Past");
        hintablePresent = GameObject.Find("Hint_Present");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !hintInProgress)
        {   
            hintLevel();
        } else if (hintInProgress)
        {
            fadeIn();
        }
    }

    private void hintLevel()
    {
        hintInProgress = true;
        presentOrPast = switchTime.switchingToPresent;
        fadeOutFactor = 0;
        if (presentOrPast)
        {
            fadeIn();
        }
        
    }

    private void fadeIn()
    {
        GameObject hintable;
        if (presentOrPast)
        {
            hintable = hintablePast;
        } else
        {
            hintable = hintablePresent;
        }

        fadeOutFactor = fadeOutFactor + Time.deltaTime;
        foreach (Transform child in hintable.transform)
        {
            Color currentColor = child.gameObject.GetComponent<Renderer>().material.color;
            currentColor.a = Mathf.Max(0, 1 - fadeOutFactor);
            child.gameObject.GetComponent<Renderer>().material.color = currentColor;
        }

        if (fadeOutFactor > 1)
        {
            hintInProgress = false;
        }
    }
}
