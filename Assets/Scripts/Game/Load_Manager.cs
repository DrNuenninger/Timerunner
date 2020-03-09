using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Manager : MonoBehaviour //Loads and manages available Save-Games
{
    public static Load_Manager load_Manager;
    void Start()
    {
        if (load_Manager)
        {
            Destroy(this);
        }
        else
        {
            load_Manager = this;
        }
    }
    public void loadDemo()
    {
        SceneManager.LoadScene("Demo_Level", LoadSceneMode.Single);
    }
    public void escape_Action()
    {
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }
}
