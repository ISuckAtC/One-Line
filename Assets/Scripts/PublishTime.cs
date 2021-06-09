using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PublishTime : MonoBehaviour
{

    public InputField NameField;

    public void publish()
    {

        GameControl.PostTime(NameField.text, SaveAndLoad.LoadGameData().TotalRunTime, false);
        SceneManager.LoadScene(0);

    }

}
