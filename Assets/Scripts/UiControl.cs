using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    private GameObject PauseGameUi;
    private GameObject InGameUi;
    public bool PauseGameUiOnOff;
    private GameControl gc;
    private Text coinsText;
    private Image regNum, iceNum, rubNum, weightNum;
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
        regGlow = GameObject.Find("Regular_Glow").GetComponent<RawImage>();
        iceGlow = GameObject.Find("Ice_Glow").GetComponent<RawImage>();
        rubGlow = GameObject.Find("Rubber_Glow").GetComponent<RawImage>();
        weightGlow = GameObject.Find("Weight_Glow").GetComponent<RawImage>();
        invert = 1;
        regNum = GameObject.Find("NumberGraphicReg").GetComponent<Image>();
        iceNum = GameObject.Find("NumberGraphicIce").GetComponent<Image>();
        rubNum = GameObject.Find("NumberGraphicRub").GetComponent<Image>();
        weightNum = GameObject.Find("NumberGraphicGrav").GetComponent<Image>();

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

        if(regNum != null)
        {

            if (gc.InkTypeSelected == 0) regNum.color = Color.red;
            else regNum.color = Color.white;
            if (gc.InkTypeSelected == 1) iceNum.color = Color.blue;
            else iceNum.color = Color.white;
            if (gc.InkTypeSelected == 2) rubNum.color = Color.black;
            else rubNum.color = Color.white;
            if (gc.InkTypeSelected == 3) weightNum.color = Color.green;
            else weightNum.color = Color.white;

        }

        if (fadeValue > 1) { invert = -1; alphaFadeValue = 1; }
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
