using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateLevelTime : MonoBehaviour
{

    public float[] Times;

    public void CreateNewTimes(bool loadPrevTimes)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings-1];

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {

            Times[SceneManager.GetActiveScene().buildIndex-1] = Time.timeSinceLevelLoad;

        }

        SaveAndLoad.SaveData(0 ,0 , false, false,Times, loadPrevTimes);

    }

}
