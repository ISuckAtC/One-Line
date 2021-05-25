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

        for(int i = 0; i <= resolutions.Length - 1; i++)
        {

            GameObject resButton = Instantiate(ResolutionButton, transform);
            buttons.Add(resButton);
            resButton.GetComponentInChildren<Text>().text = resolutions[i].width + "/" + resolutions[i].height;
            resButton.GetComponent<ResButton>().ThisButtonNum = i;

        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, buttons.Count * 100);

    }

}
