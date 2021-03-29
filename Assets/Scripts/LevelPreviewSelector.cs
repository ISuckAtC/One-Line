using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewSelector : MonoBehaviour
{

    public int LevelPreviewNumber;
    private MainMenuController mmc;

    void Start()
    {

        mmc = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuController>();

    }

    public void OpenPreview()
    {

        mmc.LevelPreview(LevelPreviewNumber);

    }

}
