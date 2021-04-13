using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public string[] LevelNames;
    public Sprite[] LevelPreviewImage;
    public string[] LevelPreviewText;
    public GameObject MainMenuCursor, PreviewPanel, ToggleButton;
    public Text PreviewName, PreviewText, GoToLevelButtonText, PreviewBestTimeText;
    public Image PreviewImage;
    private Vector3 mousePosition;
    private int levelNumSelected;
    private float slideInCounter, slideOutCounter;
    public float[] levelTimes;
    public float PreviewPanelSpeed, screenWidth;
    public bool inView;
    public CreateLevelTime CLT;
    private int ScreenWidth, ScreenHeight;
    public bool FullscreenToggle;

    void Start()
    {

        if(SaveAndLoad.LoadData() == null)
        {

            CLT.CreateNewTimes(false);

        }

        levelTimes = SaveAndLoad.LoadData().Times;

        screenWidth = Screen.width;
        Cursor.visible = false;

        ScreenWidth = SaveAndLoad.LoadData().ScreenWidth;
        ScreenHeight = SaveAndLoad.LoadData().ScreenHeight;
        FullscreenToggle = SaveAndLoad.LoadData().FullScreenMode;
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullscreenToggle);

    }

    void Update()
    {

        mousePosition = Input.mousePosition;
        MainMenuCursor.transform.position = mousePosition;

    }

    public void LevelPreview(int lvlNum)
    {

        if(lvlNum != levelNumSelected)
        {

            levelNumSelected = lvlNum;
        
            if(!inView) StartCoroutine(SlideIn(true));
            else StartCoroutine(SlideOut(true, true));

        }

    }

    public void FullscreenToggleButton()
    {

        ToggleButton.GetComponent<Toggle>().isOn = FullscreenToggle;

    }

    public void SetWidth(int Width) => ScreenWidth = Width;

    public void SetHeight(int Height) => ScreenHeight = Height;

    public void FullscreenSwitch() => FullscreenToggle = ToggleButton.GetComponent<Toggle>().isOn;

    public void ApplySettings()
    {

        Screen.SetResolution(ScreenWidth, ScreenHeight, FullscreenToggle);
        SaveAndLoad.SaveData(ScreenWidth, ScreenHeight, FullscreenToggle, true, new float[SceneManager.sceneCountInBuildSettings-1], false);

    }

    public void GoToLevel()
    {

        SceneManager.LoadScene(levelNumSelected);

    }

    private IEnumerator SlideIn(bool reset)
    {

        if(reset)
        {

            PreviewName.text = LevelNames[levelNumSelected-1];
            PreviewImage.sprite = LevelPreviewImage[levelNumSelected-1];
            PreviewText.text = LevelPreviewText[levelNumSelected-1];
            PreviewBestTimeText.text = "Best Time: " + levelTimes[levelNumSelected-1];
            GoToLevelButtonText.text = "Play Level: " + levelNumSelected;

            slideInCounter = 1;
            inView = false;

        }

        if(slideInCounter > 0)
        {

            slideInCounter -= Time.deltaTime * PreviewPanelSpeed;
            slideInCounter = Mathf.Clamp(slideInCounter, 0, 1);
            PreviewPanel.transform.position = new Vector2(screenWidth + (500 * slideInCounter), PreviewPanel.transform.position.y);
            yield return new WaitForEndOfFrame();
            StartCoroutine(SlideIn(false));

        }
        else inView = true;
    
    }

    private IEnumerator SlideOut(bool outToIn, bool reset)
    {

        if(!inView)
        {

            StopCoroutine(SlideOut(false, false));

        }

        if(reset) slideOutCounter = 0;

        if(slideOutCounter < 1)
        {

            slideOutCounter += Time.deltaTime * PreviewPanelSpeed;
            slideOutCounter = Mathf.Clamp(slideOutCounter, 0, 1);
            PreviewPanel.transform.position = new Vector2(screenWidth + (500 * slideOutCounter), PreviewPanel.transform.position.y);
            yield return new WaitForEndOfFrame();
            StartCoroutine(SlideOut(outToIn, false));

        }
        else
        {

            inView = false;
            StartCoroutine(SlideIn(true));

        }

    }

    public void QuitGame() => Application.Quit();

}