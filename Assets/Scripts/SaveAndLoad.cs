using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveAndLoad : MonoBehaviour
{

    public static void SaveGameData(float[] times, int lvlUnlockData, bool loadPrevTimes)
    {

        GameData previousLevelTimes = null;
        if(loadPrevTimes)
        {

            previousLevelTimes = LoadGameData();

        }
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneLineGameData.boron");
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        
        GameData data = new GameData(times, lvlUnlockData, previousLevelTimes);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static GameData LoadGameData()
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

    public static void SaveSettingsData(int width, int height, bool fullscreen, bool fullscreenOverWrite)
    {

        SettingsData previousData = null;
        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneLineSettingsData.boron");
        if(File.Exists(savePath))
            previousData = LoadSettingsData();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        SettingsData data = new SettingsData(width, height, fullscreen, fullscreenOverWrite, previousData);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static SettingsData LoadSettingsData()
    {

        string loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneLineSettingsData.boron");
        if(File.Exists(loadPath))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open, FileAccess.Read);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;

        }
        else
        {

            Debug.LogError("No settingsData found");
            return null;

        }

    }

}
