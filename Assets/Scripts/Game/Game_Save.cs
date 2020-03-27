using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Save : MonoBehaviour
{
    public static Game_Save gameSave;
    public int currentLevel;
    public List<Level_Save> levelList = new List<Level_Save>();
    public string levelPath = "";

    void Start()
    {
        if (gameSave)
        {
            Destroy(this);
        }
        else
        {
            gameSave = this;
        }
        loadLevelList();
    }

    public void loadLevelList(string levelPath)//loads the set of created levels into the ArrayList; gets called on Start of the Game
    {
        
    }
    public void loadLevelList()
    {
        Level_Save demoLevel = new Level_Save();
        demoLevel.levelname = "Demo_Level";
        levelList.Add(demoLevel);
        Level_Save level1 = new Level_Save();
        level1.levelname = "RaycastingMovement_DevRoom";
        levelList.Add(level1);
    }
}
