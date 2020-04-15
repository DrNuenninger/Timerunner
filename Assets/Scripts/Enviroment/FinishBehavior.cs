using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehavior : MonoBehaviour
{
    Load_Manager loadManagerController;
    Level_Information level_Information;
    StartGame startGameScript;
    HighscoreTable highscoreTable;

    void Start()
    {
        loadManagerController = GameObject.Find("LoadManagerController").GetComponent<Load_Manager>();
        level_Information = GameObject.Find("Level_Information").GetComponent<Level_Information>();
        startGameScript = GameObject.Find("Main Camera").GetComponent<StartGame>();
        highscoreTable = new HighscoreTable();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            highscoreTable.AddHighscoreEntry((int) startGameScript.timeGone,level_Information.sceneName);
            loadManagerController.loadNextLevel(); 
        }
    }
}
