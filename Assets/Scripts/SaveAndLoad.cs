using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoad : MonoBehaviour
{

    public static void SaveTimes(CreateLevelTime CLT)
    {

        LevelTimes previousTimes = LoadTimes();
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = "C:/PlayerData.kevo";
        FileStream stream = new FileStream(savePath, FileMode.Create);

        LevelTimes data = new LevelTimes(CLT, previousTimes);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static LevelTimes LoadTimes()
    {

        string loadPath = "C:/PlayerData.kevo";
        if(File.Exists(loadPath))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open);

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
