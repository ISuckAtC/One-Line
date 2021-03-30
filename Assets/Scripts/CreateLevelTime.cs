using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateLevelTime : MonoBehaviour
{

    public float[] Times;

    void Start()
    {

        Times = new float[SceneManager.sceneCountInBuildSettings-1];

    }

    public void CreateNewTimes()
    {

        Times[SceneManager.GetActiveScene().buildIndex-1] = Time.timeSinceLevelLoad;
        SaveAndLoad.SaveTimes(this);

    }

}
