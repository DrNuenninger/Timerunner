using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Manager : MonoBehaviour //Loads and manages available Save-Games
{
    public void loadDemo()
    {
        SceneManager.LoadScene("Demo_Level", LoadSceneMode.Single);
    }
}
