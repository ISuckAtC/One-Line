using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{

    public int ScreenWidth, ScreenHeight;
    public bool FullScreenMode;

    public SettingsData(int Width, int Height ,bool FullScreenToggle, bool OverWriteFullscreen, SettingsData PreviousData)
    {

        if(OverWriteFullscreen)
            FullScreenMode = FullScreenToggle;
        else
        {

            if(PreviousData != null)
                FullScreenMode = PreviousData.FullScreenMode;
            else
                FullScreenMode = true;

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

    }

}
