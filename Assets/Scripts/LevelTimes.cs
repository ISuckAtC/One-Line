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
    public LevelTimes(CreateLevelTime NewLT, LevelTimes PreviousTimes)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings-1];

        if(PreviousTimes != null)
        {
            for(int i = 0; i < Times.Length-1; i++)
            {

                if(NewLT.Times[i] < PreviousTimes.Times[i] && NewLT.Times[i] != 0)
                {

                    Times[i] = NewLT.Times[i];

                }
                else if(PreviousTimes.Times[i] != 0)
                {

                    Times[i] = PreviousTimes.Times[i];

                }
                else
                {

                    Times[i] = 0;

                }

            }

        }
        else
        {

            Times = NewLT.Times;

        }

    }

}
