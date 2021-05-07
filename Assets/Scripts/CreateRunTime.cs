using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRunTime : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            if(GameObject.FindGameObjectWithTag("Timer"))
            {

                SaveAndLoad.SaveGameData(new float[0], GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>().runTime, 0, true);

            }

    }

}
