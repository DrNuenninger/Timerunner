using UnityEngine;

public class HintLevel : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    GameObject hintablePast;
    GameObject hintablePresent;
    SwitchTime switchTime;
    bool presentOrPast = true;
    bool hintInProgress = false;
    float fadeInFactor;
    int fadeInSeconds = 4;

    void Start()
    {
        switchTime = gameObject.GetComponent<SwitchTime>();
        hintablePast = GameObject.Find("Hint_Past");
        hintablePresent = GameObject.Find("Hint_Present");
    }

    // Update is called once per frame
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
        presentOrPast = switchTime.presentOrPast;
        fadeInFactor = 0;
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

        fadeInFactor = fadeInFactor + Time.deltaTime;
        foreach (Transform child in hintable.transform)
        {
            Color currentColor = child.gameObject.GetComponent<Renderer>().material.color;
            currentColor.a = Mathf.Max(0, 1 - fadeInFactor);
            child.gameObject.GetComponent<Renderer>().material.color = currentColor;
        }

        if (fadeInFactor > 1)
        {
            hintInProgress = false;
        }
    }
}
