using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    public static UiControl main { get; private set;}
    public Vector3 MinimizedSize, EnlargedSize;
    private GameObject PauseGameUi;
    private GameObject InGameUi;
    public bool PauseGameUiOnOff;
    private GameControl gc;
    public GameObject[] Inkwells, InkwellPositions;
    private Text coinsText;
    private Text levelSceneNumber;
    public BarController[] BarControllers;
    public GameObject DialogueBoxHero, DialogueBoxEnemy;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameControl.main;
        Scene scene = SceneManager.GetActiveScene();
        levelSceneNumber = GameObject.Find("LevelNumber").GetComponent<Text>();
        levelSceneNumber.text = "level " + scene.buildIndex.ToString();
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        PauseGameUiOnOff = false;
        PauseGameUi = GameObject.Find("PauseGameUi");
        InGameUi = GameObject.Find("InGameUi");
        PauseGameUi.SetActive(false);

    }

    void Awake() 
    {

        main = this;

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape)) SwitchUi();

    }

    private void SwitchUi() 
    {

        PauseGameUiOnOff = !PauseGameUiOnOff;
        if (InGameUi)
        {

            PauseGameUi.SetActive(PauseGameUiOnOff);
            InGameUi.SetActive(!PauseGameUiOnOff);
            coinsText.GetComponent<Text>().text = "coins: " + gc.Global.Coins;

        }

    }

    public void UpdateUi() 
    {

        foreach(BarController bc in BarControllers) 
        {

            bc.UpdateInkBar();

        }

        for(int i = 0; i <= Inkwells.Length - 1; i++) 
        {

            if(GameControl.main.InkTypeSelected == i)
            {
                
                Inkwells[i].transform.localScale = EnlargedSize;
                Inkwells[i].GetComponent<RectTransform>().position = InkwellPositions[4].GetComponent<RectTransform>().position;;
                
            }
            else 
            {
                
                Inkwells[i].transform.localScale = MinimizedSize;
                Inkwells[i].GetComponent<RectTransform>().position = InkwellPositions[i].GetComponent<RectTransform>().position;
                
            }

        }

    }

    public void MainMenuButton() => SceneManager.LoadScene(0);

    public void QuitButton() 
    {

        if (gc.Global != null) gc.Global.ResetCount += 1;
        Application.Quit();
        Debug.Log("I do be working - (The Quit Button).");

    }

    public void LevelSelect(int lvlToLoad) => SceneManager.LoadScene(lvlToLoad);

    public void NextLevel()
    {

        if (SceneManager.sceneCountInBuildSettings == (SceneManager.GetActiveScene().buildIndex + 1)) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ReplayLevel() 
    {

        gc.Global.ResetCount += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    } 
}
