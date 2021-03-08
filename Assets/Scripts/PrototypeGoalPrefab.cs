using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeGoalPrefab : MonoBehaviour
{

    public GameObject fireworks, confettiCanon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player") 
        {

            fireworks.SetActive(true);
            confettiCanon.SetActive(true);

        }

    }

}
