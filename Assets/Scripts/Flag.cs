using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public string nextScene;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameControl.main.LevelCompleted = true;
            StartCoroutine(GameControl.main.EndTravel(nextScene));
        }
    }
}
