using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public bool ExcludeLevelNames;
    public string[] LevelNames;
    public bool ExcludeLevelImages;
    public Sprite[] LevelPreviewImage;
    public bool ExcludeLevelTexts;
    public string[] LevelPreviewText;
    public GameObject MainMenuCursor, PreviewPanel, ToggleButton;
    public Text PreviewName, PreviewText, GoToLevelButtonText, HardmodeLevelButtonText,PreviewBestTimeText;
    public Button HardmodeLevelButton;
    public Image PreviewImage;
    private Vector3 mousePosition;
    private int levelNumSelected, hardmodeLevelsUnlocked;
    private float slideInCounter, slideOutCounter;
    public bool ExcludeLevelTimes;
    public float[] levelTimes;
    public float PreviewPanelSpeed, screenWidth;
    public bool inView;
    public CreateLevelTime CLT;
    private int ScreenWidth, ScreenHeight;
    public bool FullscreenToggle;
    private SettingsData settingsData;
    private GameData gameData;

    void Start()
    {

        if(SaveAndLoad.LoadGameData() == null)
            CLT.CreateNewTimes(false);

        if(SaveAndLoad.LoadSettingsData() == null)
            SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, FullscreenToggle, false);

        gameData = SaveAndLoad.LoadGameData();

        settingsData = SaveAndLoad.LoadSettingsData();

        if(levelTimes.Length < SceneManager.sceneCountInBuildSettings - 1)
        {

            CLT.CreateNewTimes(false);
            Debug.Log("Updating saved data...");
            levelTimes = SaveAndLoad.LoadGameData().Times;

        }

        screenWidth = Screen.width;
        Cursor.visible = false;

        ScreenWidth = settingsData.ScreenWidth;
        ScreenHeight = settingsData.ScreenHeight;
        FullscreenToggle = settingsData.FullScreenMode;
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullscreenToggle);
        
        if(ExcludeLevelNames)
            LevelNames = new string[SceneManager.sceneCountInBuildSettings];

        if(ExcludeLevelImages)
            LevelPreviewImage = new Sprite[SceneManager.sceneCountInBuildSettings];

        if(ExcludeLevelTexts)
            LevelPreviewText = new string[SceneManager.sceneCountInBuildSettings];

        levelTimes = gameData.Times;
        
        hardmodeLevelsUnlocked = gameData.LevelsUnlocked - LevelNames.Length - 2;

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
        SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, FullscreenToggle, true);
        screenWidth = ScreenWidth;

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

            if(hardmodeLevelsUnlocked >= levelNumSelected)
            {

                if(SceneManager.sceneCount < levelNumSelected)
                {

                    HardmodeLevelButton.interactable = false;
                    HardmodeLevelButtonText.text = "No more levels";

                }
                else
                {

                    HardmodeLevelButton.interactable = true;
                    HardmodeLevelButtonText.text = "Play Level" + levelNumSelected + LevelNames.Length;

                }

            }
            else
            {

                HardmodeLevelButton.interactable = false;

            }

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

    public void UnlockAllLevels()
    {

        hardmodeLevelsUnlocked = 100;

        StartCoroutine(SlideIn(true));

    }

    public void EraseGameData()
    {

        CLT.CreateNewTimes(false);
        hardmodeLevelsUnlocked = gameData.LevelsUnlocked - LevelNames.Length - 2;

    }

    public void QuitGame() => Application.Quit();

}