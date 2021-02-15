using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlSelectButton : MonoBehaviour
{

    private UiControl uiController;
    public int lvlNumber;

    private void Start()
    {

        uiController = GameObject.FindObjectOfType<Canvas>().GetComponent<UiControl>();

    }

    public void lvlButton()
    {

        uiController.LevelSelect(lvlNumber);

    }

}
