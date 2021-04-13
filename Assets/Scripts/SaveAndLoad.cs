using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveAndLoad : MonoBehaviour
{

    public static void SaveData(int ScreenWidth, int ScreenHeight, bool FullscreenToggle, bool OverWrite, float[] times, bool loadPrevTimes)
    {

        GameData previousLevelTimes = null;
        if(loadPrevTimes)
        {

            previousLevelTimes = LoadData();

        }
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneLineGameData.boron");
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        
        GameData data = new GameData(ScreenWidth , ScreenHeight, FullscreenToggle, OverWrite, times, previousLevelTimes);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static GameData LoadData()
    {

        string loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneLineGameData.boron");
        if(File.Exists(loadPath))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open, FileAccess.Read);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;

        }
        else
        {

            Debug.LogError("No file found");
            return null;

        }

    }

}
