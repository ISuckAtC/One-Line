﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    public Vector3 MinimizedSize, EnlargedSize;
    public RectTransform Selected, NotSelected;
    private GameObject PauseGameUi;
    private GameObject InGameUi;
    public bool PauseGameUiOnOff;
    private GameControl gc;
    private Text coinsText;
    [SerializeField]
    private GameObject[] InkWells;
    private RawImage regGlow, iceGlow, rubGlow, weightGlow;
    private float fadeValue, invert, alphaFadeValue;
    private LineType CLT; //CurrentLineType
    private RawImage CGT; //CurrentGlowType
    private Text levelSceneNumber;

    // Start is called before the first frame update
    void Start()
    {

        Scene scene = SceneManager.GetActiveScene(); ;
        levelSceneNumber = GameObject.Find("LevelNumber").GetComponent<Text>();
        levelSceneNumber.text = "level " + scene.buildIndex.ToString();
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        gc = GameControl.main;
        PauseGameUiOnOff = false;
        PauseGameUi = GameObject.Find("PauseGameUi");
        InGameUi = GameObject.Find("InGameUi");
        /*regGlow = GameObject.Find("Regular_Glow").GetComponent<RawImage>();
        iceGlow = GameObject.Find("Ice_Glow").GetComponent<RawImage>();
        rubGlow = GameObject.Find("Rubber_Glow").GetComponent<RawImage>();
        weightGlow = GameObject.Find("Weight_Glow").GetComponent<RawImage>();
        invert = 1;*/

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGameUiOnOff = !PauseGameUiOnOff;

        if (InGameUi)
        {

            PauseGameUi.SetActive(PauseGameUiOnOff);
            InGameUi.SetActive(!PauseGameUiOnOff);
            coinsText.GetComponent<Text>().text = "coins: " + gc.Coins;

        }

        for(int i = 0; i <= InkWells.Length - 1; i++) 
        {

            if(gc.InkTypeSelected == i)
            {
                
                InkWells[i].transform.localScale = EnlargedSize;
                InkWells[i].GetComponent<RectTransform>().position = Selected.position;
                
            }
            else 
            {
                
                InkWells[i].transform.localScale = MinimizedSize;
                InkWells[i].GetComponent<RectTransform>().position = new Vector2(NotSelected.position.x, NotSelected.position.y - (i * 30));
                
            }

        }

        /*if (fadeValue > 1) { invert = -1; alphaFadeValue = 1; }
        else if (fadeValue < 0) { invert = 1; alphaFadeValue = 0; }
        fadeValue += Time.unscaledDeltaTime * invert;

        CLT = LineType.Normal;
        CGT = regGlow;
        if (gc.Ink[(int)CLT] <= 0) CGT.enabled = false;
        else if (gc.Ink[(int)CLT] == 1) { CGT.enabled = true; CGT.CrossFadeAlpha(alphaFadeValue, 1, true); }
        else { CGT.enabled = true; CGT.CrossFadeAlpha(1, 1, true); }

        CLT = LineType.Ice;
        CGT = iceGlow;
        if (gc.Ink[(int)CLT] <= 0) CGT.enabled = false;
        else if (gc.Ink[(int)CLT] == 1) { CGT.enabled = true; CGT.CrossFadeAlpha(alphaFadeValue, 1, true); }
        else { CGT.enabled = true; CGT.CrossFadeAlpha(1, 1, true); }

        CLT = LineType.Rubber;
        CGT = rubGlow;
        if (gc.Ink[(int)CLT] <= 0) CGT.enabled = false;
        else if (gc.Ink[(int)CLT] == 1) { CGT.enabled = true; CGT.CrossFadeAlpha(alphaFadeValue, 1, true); }
        else { CGT.enabled = true; CGT.CrossFadeAlpha(1, 1, true); }

        CLT = LineType.Weight;
        CGT = weightGlow;
        if (gc.Ink[(int)CLT] <= 0) CGT.enabled = false;
        else if (gc.Ink[(int)CLT] == 1) { CGT.enabled = true; CGT.CrossFadeAlpha(alphaFadeValue, 1, true); }
        else { CGT.enabled = true; CGT.CrossFadeAlpha(1, 1, true); }
*/

    }

    public void MainMenuButton() => SceneManager.LoadScene(0);

    public void QuitButton() => Application.Quit();

    public void LevelSelect(int lvlToLoad) => SceneManager.LoadScene(lvlToLoad);

    public void NextLevel()
    {

        if (SceneManager.sceneCountInBuildSettings == (SceneManager.GetActiveScene().buildIndex + 1)) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ReplayLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}
