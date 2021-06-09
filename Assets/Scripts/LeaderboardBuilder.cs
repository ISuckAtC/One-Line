using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBuilder : MonoBehaviour
{

    public int FontSize, LeaderboardSize;
    public Font LeaderboardFont;
    public string[] strings;
    private bool empty;

    void Awake()
    {

        LoadLeaderboard();

    }

    public void LoadLeaderboard()
    {

        int screenWidth = SaveAndLoad.LoadSettingsData().ScreenWidth;

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 100 + ((LeaderboardSize + (LeaderboardSize / 100)) * (FontSize + (FontSize / 5))));

        for(int i = 0; i < LeaderboardSize; i++)
        {

            empty = false;

            Text tempText = new GameObject("LeaderboardText " + i).AddComponent<Text>();
            tempText.transform.SetParent(transform);
            tempText.fontSize = FontSize;
            tempText.horizontalOverflow = HorizontalWrapMode.Overflow;
            tempText.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, FontSize + (FontSize / 5));
            tempText.font = LeaderboardFont;
            tempText.alignment = TextAnchor.MiddleCenter;

            if(strings.Length > i)
                if(strings[i] != null && strings[i] != "")
                {

                    tempText.text = strings[i];

                }
                else
                    empty = true;
            else
                empty = true;

            
            if(empty)
            {

                tempText.text = "--:--:--";

            }

        }

    }

}
