using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateLevelTime : MonoBehaviour
{

    public float[] Times;
    public int levelNum;

    public void CreateNewTimes(bool loadPrevTimes)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings - 1];

        levelNum = SceneManager.GetActiveScene().buildIndex;

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {

            Times[SceneManager.GetActiveScene().buildIndex - 1] = Time.timeSinceLevelLoad;

        }

        

        SaveAndLoad.SaveGameData(Times, 0, levelNum, loadPrevTimes, GameControl.main ? GameControl.main.Global.TotalRunTime : 0);
    }
}
