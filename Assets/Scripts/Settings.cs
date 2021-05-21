using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public GameObject ResolutionButton;
    public Resolution[] resolutions;
    private List<GameObject> buttons;

    void Start()
    {

        resolutions = Screen.resolutions;

        buttons = new List<GameObject>();

        buttons.Add(ResolutionButton);

        for(int i = 0; i < resolutions.Length - 2; i++)
        {

            GameObject resButton = Instantiate(ResolutionButton, transform);
            buttons.Add(resButton);

        }

    }

}
