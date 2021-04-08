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

        for(int i = 0; i < Times.Length - 1; i++)
        {

            Debug.Log("CLT: " + Times[i]);

        }

        SaveAndLoad.SaveTimes(Times, loadPrevTimes);
        Debug.Log("Saving...");

    }

}
