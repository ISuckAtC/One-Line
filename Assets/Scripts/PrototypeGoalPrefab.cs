using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeGoalPrefab : MonoBehaviour
{

    private UiControl uiController;

    private void Start()
    {

        uiController = GameObject.FindObjectOfType<Canvas>().GetComponent<UiControl>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
            uiController.LevelFinish();

    }

}
