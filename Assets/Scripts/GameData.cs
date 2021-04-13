using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{

    public float[] Times;
    public int ScreenWidth, ScreenHeight;
    public bool FullScreenMode;

    //Used the values from both the previously saved Times array and the one currently being saved
    //Checks if the new times are better or worse and selects the best, if the time spent is 0, it will not save anything
    public GameData(int Width, int Height ,bool FullScreenToggle, bool OverWriteFullscreen,float[] NewLT, GameData PreviousData)
    {

        Times = new float[SceneManager.sceneCountInBuildSettings];
        float[] newTime = NewLT;

        if(OverWriteFullscreen)
            FullScreenMode = FullScreenToggle;
        else
        {

            if(PreviousData != null)
                FullScreenMode = PreviousData.FullScreenMode;
            else
                FullScreenMode = false;

        }

        if(PreviousData != null)
        {

            if(Width != 0 && Height != 0)
            {

                ScreenWidth = Width;
                ScreenHeight = Height;

            }
            else if(PreviousData.ScreenWidth <= 0 || PreviousData.ScreenHeight <= 0)
            {

                ScreenWidth = 1920;
                ScreenHeight = 1080;

            }
            else
            {

                ScreenWidth = PreviousData.ScreenWidth;
                ScreenHeight = PreviousData.ScreenHeight;

            }

        }
        else
        {

            if(Width != 0 && Height != 0)
            {

                ScreenWidth = Width;
                ScreenHeight = Height;

            }
            else
            {

                ScreenWidth = 1920;
                ScreenHeight = 1080;

            }

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
