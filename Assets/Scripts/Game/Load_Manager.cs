using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Manager : MonoBehaviour //Loads and manages available Save-Games
{
    public static Load_Manager load_Manager;
    private Level_Information LevelInformation;
    private Dictionary<string, int> sceneList = new Dictionary<string, int>();

    void Start()
    {
        load_Manager = this;
        LevelInformation = GameObject.Find("Level_Information").GetComponent<Level_Information>();

        sceneList.Add("Main_Menu", 0);
        sceneList.Add("Level_Select", 1);
        sceneList.Add("Forest", 2);
        sceneList.Add("City", 3);
        sceneList.Add("Military-Base", 4);
        sceneList.Add("Demo_Level", 100);
    }
   
    public void escape_Action()
    {
        load_mainMenu();
    }

    public void exit_Game()
    {
        Application.Quit();
    }

    public void load_LevelSelect()
    {
        SceneManager.LoadScene("Level_Select", LoadSceneMode.Single);
    }

    public void load_Leaderboards()
    {
        SceneManager.LoadScene("Leaderboards");
    }

    public void load_mainMenu()
    {
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }

    public void load_Level(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void loadNextLevel()
    {
        int currentId = sceneList[LevelInformation.sceneName];

        foreach (KeyValuePair<string, int> entry in sceneList)
        {
            if (entry.Value == currentId +1)
            {
                load_Level(entry.Key);
                return;
            }
        }
        load_LevelSelect();
    }
}
