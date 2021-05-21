using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateRunTime : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            if(GameObject.FindGameObjectWithTag("Timer"))
            {
                SaveAndLoad.SaveGameData(new float[SceneManager.sceneCountInBuildSettings - 1], GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>().runTime, 0, true, GameControl.main.Global.TotalRunTime);

            }
    }
}
