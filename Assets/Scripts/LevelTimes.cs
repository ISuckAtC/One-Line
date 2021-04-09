using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelTimes
{

    public float[] Times;

    //Used the values from both the previously saved Times array and the one currently being saved
    //Checks if the new times are better or worse and selects the best, if the time spent is 0, it will not save anything
    public LevelTimes(float[] NewLT, LevelTimes previousLevelTimes)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings-1];
        float[] newTime = NewLT;

        for (int i = 0; i < newTime.Length - 1; i++)
        {

            if(previousLevelTimes != null && i < previousLevelTimes.Times.Length - 1)
            {

                if(newTime[i] < previousLevelTimes.Times[i] && newTime[i] != 0)
                {

                    Times[i] = newTime[i];

                }
                else if(previousLevelTimes.Times[i] != 0)
                {

                    Times[i] = previousLevelTimes.Times[i];

                }
                else
                {

                    Times[i] = newTime[i];

                }

            }
            else
            {

                Times[i] = newTime[i];

            }

        }

    }

}
