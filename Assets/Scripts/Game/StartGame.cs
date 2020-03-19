using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public float timeGone = 0.0f;
    public Text startText; // used for showing countdown from 3, 2, 1 
    public bool timer_running = true;

    void Update()
    {
        if (timer_running)
        {
            timeGone += Time.deltaTime;
            startText.text = (timeGone).ToString("0");
        }
        
    }
    public void stopTimer()
    {
        timer_running = false;
    }
}