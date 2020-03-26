using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Manager : MonoBehaviour //Loads and manages available Save-Games
{
    public static Load_Manager load_Manager;
    public string currentLevel = "Main_Menu";
    public int currentLevelIndex = 0;
    public Game_Save gs; //game_Save überall durch gs.gameSave ersetzen!
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
        //Game_Save.loadLevelList();
        SceneManager.LoadScene("Level_Select", LoadSceneMode.Single);
    }
    public void load_Level(string scenename)
    {
        //if (Game_Save.levelList != null)
        //{
            SceneManager.LoadScene(scenename, LoadSceneMode.Single);
            //Level_Save ls = (Level_Save)Game_Save.levelList[0];
            //print(ls.levelname);
        //}
        
           SceneManager.LoadScene(scenename, LoadSceneMode.Single);
    }
    /*public void loadNextLevel()
    {
        currentLevelIndex++;
        if (Game_Save.levelList[currentLevelIndex] != null)
        {
            string nextName = Game_Save.levelList[currentLevelIndex].levelName;
            currentLevel = nextName;
            SceneManager.LoadScene(nextName);
        }
    }*/
}
