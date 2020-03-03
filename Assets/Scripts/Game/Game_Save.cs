using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Save : MonoBehaviour
{
    public int currentLevel;
    public ArrayList levelList = new ArrayList();
    public string levelPath = "";

    public void loadLevelList(string levelPath)
    {
        //loads the set of created levels into the ArrayList; gets called on Start of the Game
    }
}
