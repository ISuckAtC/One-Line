using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBuilder : MonoBehaviour
{

    public int FontSize, LeaderboardSize, SpacingInLayoutGroup;
    public Font LeaderboardFont;
    public string[] strings;
    [SerializeField]
    private List<Text> texts;
    private bool empty;
    [SerializeField]
    private MainMenuController mmc;

    void Awake()
    {

        mmc = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuController>();
        texts = new List<Text>();

    }

    void OnEnable()
    {

        LoadLeaderboard();

    }

    public void LoadLeaderboard()
    {

        if(texts.Count > 0)
        {

            foreach (Text t in texts)
            {

                Destroy(t.gameObject);
                
            }

        }

        texts = new List<Text>();

        strings = mmc.GetLeaderBoards(false);

        int screenWidth = SaveAndLoad.LoadSettingsData().ScreenWidth;

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 100 + ((LeaderboardSize + (LeaderboardSize / 100)) * (FontSize + (FontSize / 5) + SpacingInLayoutGroup)));

        for(int i = 0; i < LeaderboardSize; i++)
        {

            empty = false;

            Text tempText = new GameObject("LeaderboardText " + i).AddComponent<Text>();
            texts.Add(tempText);
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
