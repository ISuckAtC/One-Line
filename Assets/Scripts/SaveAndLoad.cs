using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveAndLoad : MonoBehaviour
{

    public static void SaveTimes(float[] times, bool loadPrevTimes)
    {

        LevelTimes previousLevelTimes = null;
        if(loadPrevTimes)
        {

            previousLevelTimes = LoadTimes();

        }
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameData.kevo");
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        
        LevelTimes data = new LevelTimes(times, previousLevelTimes);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static LevelTimes LoadTimes()
    {

        string loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameData.kevo");
        if(File.Exists(loadPath))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open, FileAccess.Read);

            LevelTimes data = formatter.Deserialize(stream) as LevelTimes;
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
