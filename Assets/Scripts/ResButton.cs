using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResButton : MonoBehaviour
{

    public int ThisButtonNum;
    private Resolution[] res;
    private MainMenuController mMC;

    void Start()
    {

        mMC = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuController>();
        res = Screen.resolutions;
        gameObject.GetComponent<Button>().onClick.AddListener(SetResolution);

    }

    public void SetResolution()
    {

        mMC.SetWidth(res[ThisButtonNum].width);
        mMC.SetHeight(res[ThisButtonNum].height);

    }

}
