using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{

    public float[] Times;
    public int LevelsUnlocked;
    public float BestRun, RecentRun;

    //Uses the values from both the previously saved Times array and the one currently being saved
    //Checks if the new times are better or worse and selects the best, if the time spent is 0, it will not save anything
    public GameData(float[] NewLT, float RunTime, int levelUnlockValue, GameData PreviousData)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings - 1];
        float[] newTime = NewLT;
        
        if(PreviousData != null)
        {

            if(RunTime != 0)
                RecentRun = RunTime;
            else
                RecentRun = PreviousData.RecentRun;

            if(levelUnlockValue >= PreviousData.LevelsUnlocked)
            {

                LevelsUnlocked = levelUnlockValue;

            }
            else
            {

                LevelsUnlocked = PreviousData.LevelsUnlocked;

            }

            if(RunTime != 0)
            {

                if(RunTime <= PreviousData.BestRun)
                {

                    BestRun = PreviousData.BestRun;

                }
                else
                {

                    BestRun = RunTime;

                }

            }
            else
                BestRun = PreviousData.BestRun;

        }
        else
        {

            LevelsUnlocked = levelUnlockValue;
            BestRun = RunTime;
            RecentRun = RunTime;

        }

        for (int i = 0; i < newTime.Length - 1; i++)
        {

            if(PreviousData != null && i < PreviousData.Times.Length - 1)
            {

                if(newTime[i] < PreviousData.Times[i] && newTime[i] != 0)
                {

                    Times[i] = newTime[i];

                }
                else if(PreviousData.Times[i] != 0)
                {

                    Times[i] = PreviousData.Times[i];

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
