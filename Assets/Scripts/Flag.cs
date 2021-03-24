using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public int nextSceneIndex;
    public bool GoToNext = true;
    public GameObject fireworks, confettiCanon;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            fireworks.SetActive(true);
            confettiCanon.SetActive(true);
            GameControl.main.LevelCompleted = true;
            StartCoroutine(GameControl.main.EndTravel(GoToNext ? SceneManager.GetActiveScene().buildIndex + 1 : nextSceneIndex));
        }
    }
}
