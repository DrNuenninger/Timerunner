using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class KillPlayerOnImpact : MonoBehaviour
{
    Controller2D controller2DPlayer;
    GameObject player;

    void Start()
    {
        controller2DPlayer = GameObject.Find("Player").GetComponent<Controller2D>();
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (this.tag == "BarbedWire") {
                controller2DPlayer.isSplashed = true;
                sendBarbWireDeath();
            }
            controller2DPlayer.isDead = true;
            sendOtherDeath();
        }
    }

    void sendBarbWireDeath()
    {
        
        UnityEngine.Analytics.Analytics.CustomEvent("player_death", new Dictionary<string, object>
        {
            {"PosX", player.transform.position.x },
            {"PosY", player.transform.position.y },
            {"Reason", "BarbedWire" },
            {"Level", SceneManager.GetActiveScene().name }

        });
    }

    void sendOtherDeath()
    {

        UnityEngine.Analytics.Analytics.CustomEvent("player_death", new Dictionary<string, object>
        {
            {"PosX", player.transform.position.x },
            {"PosY", player.transform.position.y },
            {"Reason", this.name },
            {"Level", SceneManager.GetActiveScene().name }

        });
    }

   
}
