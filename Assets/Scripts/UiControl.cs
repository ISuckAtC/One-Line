using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    public static UiControl main { get; private set;}
    public Vector3 MinimizedSize, EnlargedSize;
    private Vector3 mousePosition;
    private GameObject PauseGameUi;
    private GameObject InGameUi;
    public bool PauseGameUiOnOff;
    private GameControl gc;
    public GameObject[] Inkwells, InkwellPositions;
    private Text coinsText;
    private Text levelSceneNumber;
    public BarController[] BarControllers;
    public GameObject DialogueBoxHero, DialogueBoxEnemy, RestartText, GameCursor;
    public Image CursorInkCircle;
    public Sprite NormalCursor, IceCursor, RubberCursor, GravityCursor;
    public Color32 Normal, Ice, Rubber, Gravity;
    private LineType lineTypeSelected;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameControl.main;
        Scene scene = SceneManager.GetActiveScene();
        levelSceneNumber = GameObject.Find("LevelNumber").GetComponent<Text>();
        levelSceneNumber.text = "level " + scene.buildIndex.ToString() + " - " + scene.name;
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

        mousePosition = Input.mousePosition;
        GameCursor.transform.position = mousePosition;

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

    void UpdateInkCircle()
    {

        if(gc.InkTypeSelected == 0) lineTypeSelected = LineType.Normal;
        else if(gc.InkTypeSelected == 1) lineTypeSelected = LineType.Ice;
        else if(gc.InkTypeSelected == 2) lineTypeSelected = LineType.Rubber;
        else if(gc.InkTypeSelected == 3) lineTypeSelected = LineType.Weight;

        float CurrentAmount = gc.Ink[(int)gc.InkTypeSelected];
        float ProsentOfTotal = CurrentAmount / 100;
        ProsentOfTotal = Mathf.Clamp(ProsentOfTotal, 0, 1);

        switch (lineTypeSelected)
        {
            
            case LineType.Normal:
            CursorInkCircle.color = Normal;
            GameCursor.GetComponent<Image>().sprite = NormalCursor;
            break;

            case LineType.Ice:
            CursorInkCircle.color = Ice;
            GameCursor.GetComponent<Image>().sprite = IceCursor;
            break;

            case LineType.Rubber:
            CursorInkCircle.color = Rubber;
            GameCursor.GetComponent<Image>().sprite = RubberCursor;
            break;

            case LineType.Weight:
            CursorInkCircle.color = Gravity;
            GameCursor.GetComponent<Image>().sprite = GravityCursor;
            break;

        }
        CursorInkCircle.fillAmount = ProsentOfTotal;

    }

    public void UpdateUi() 
    {

        foreach(BarController bc in BarControllers) 
        {

            bc.UpdateInkBar();

        }

        UpdateInkCircle();

        for(int i = 0; i <= Inkwells.Length - 1; i++) 
        {

            if(gc.InkTypeSelected == i)
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

        if(gc.Global != null) gc.Global.ResetCount += 1;
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
