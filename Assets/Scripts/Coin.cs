using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {

            GameControl gc = GameObject.Find("GameControl").GetComponent<GameControl>();
            gc.Coins += 1;
            Destroy(gameObject);

        }

    }

}
