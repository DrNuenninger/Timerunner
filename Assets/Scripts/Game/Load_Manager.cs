using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Manager : MonoBehaviour //Loads and manages available Save-Games
{
    public static Load_Manager load_Manager;
    public string currentLevel = "Main_Menu";
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
    public void exit_Game()
    {
        Application.Quit();
    }
    public void load_LevelSelect()
    {
        SceneManager.LoadScene("Level_Select", LoadSceneMode.Single);
    }
    public void load_Level(string scenename)
    {
        SceneManager.LoadScene(scenename, LoadSceneMode.Single);
    }
}
