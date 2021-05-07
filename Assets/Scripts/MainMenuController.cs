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
    public Text PreviewName, PreviewText, GoToLevelButtonText, HardmodeLevelButtonText, PreviewBestTimeText, PreviewBestHardTimeText, RecentRunText, BestRunText;
    public Button HardmodeLevelButton;
    public Image PreviewImage;
    private Vector3 mousePosition;
    private int levelNumSelected, hardmodeLevelsUnlocked;
    private float slideInCounter, slideOutCounter, speedrunTime;
    public bool ExcludeLevelTimes;
    public float[] levelTimes;
    public float PreviewPanelSpeed, screenWidth;
    public bool inView;
    public CreateLevelTime CLT;
    private int ScreenWidth, ScreenHeight, Framerate;
    public bool FullscreenToggle;
    private SettingsData settingsData;
    private GameData gameData;

    void Start()
    {

        if(GameObject.FindGameObjectWithTag("Timer"))
        {

            speedrunTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>().runTime;
            Destroy(GameObject.FindGameObjectWithTag("Timer"));

        }

        if(SaveAndLoad.LoadGameData() == null)
            CLT.CreateNewTimes(false);

        if(SaveAndLoad.LoadSettingsData() == null)
            SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, Framerate, FullscreenToggle, false);

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
        Framerate = settingsData.Framerate;
        Application.targetFrameRate = Framerate;
        
        if(ExcludeLevelNames)
            LevelNames = new string[SceneManager.sceneCountInBuildSettings];

        if(ExcludeLevelImages)
            LevelPreviewImage = new Sprite[SceneManager.sceneCountInBuildSettings];

        if(ExcludeLevelTexts)
            LevelPreviewText = new string[SceneManager.sceneCountInBuildSettings];

        levelTimes = gameData.Times;
        
        hardmodeLevelsUnlocked = gameData.LevelsUnlocked - (LevelNames.Length - 1);
        Debug.Log(hardmodeLevelsUnlocked);

        RecentRunText.text = "Last Run: " + gameData.RecentRun;
        BestRunText.text = "Best Run: " + gameData.BestRun;

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

    public void SetFramerate(int fps) => Framerate = fps;

    public void SetWidth(int Width) => ScreenWidth = Width;

    public void SetHeight(int Height) => ScreenHeight = Height;

    public void FullscreenSwitch() => FullscreenToggle = ToggleButton.GetComponent<Toggle>().isOn;

    public void ApplySettings()
    {

        Screen.SetResolution(ScreenWidth, ScreenHeight, FullscreenToggle);
        SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, Framerate, FullscreenToggle, true);
        screenWidth = ScreenWidth;
        Application.targetFrameRate = Framerate;

    }

    public void GoToLevel()
    {

        SceneManager.LoadScene(levelNumSelected);

    }

    public void GoToHardLevel()
    {

        SceneManager.LoadScene(levelNumSelected + LevelNames.Length);

    }

    private IEnumerator SlideIn(bool reset)
    {

        if(reset)
        {

            PreviewName.text = LevelNames[levelNumSelected-1];
            PreviewImage.sprite = LevelPreviewImage[levelNumSelected-1];
            PreviewText.text = LevelPreviewText[levelNumSelected-1];
            PreviewBestTimeText.text = "Best Time: " + levelTimes[levelNumSelected-1];
            PreviewBestHardTimeText.text = "Best Hardmode Time: " + levelTimes[levelNumSelected-1 + LevelNames.Length];
            GoToLevelButtonText.text = "Play Level: " + levelNumSelected;

            if(hardmodeLevelsUnlocked >= levelNumSelected)
            {

                if(SceneManager.sceneCountInBuildSettings < levelNumSelected)
                {

                    HardmodeLevelButton.interactable = false;
                    HardmodeLevelButtonText.text = "No more levels";

                }
                else
                {

                    HardmodeLevelButton.interactable = true;
                    HardmodeLevelButtonText.text = "Play Level" + (levelNumSelected + LevelNames.Length);

                }

            }
            else
            {

                HardmodeLevelButton.interactable = false;
                HardmodeLevelButtonText.text = "Level Not Unlocked";

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

    public void UnlockFirstLevel()
    {

        hardmodeLevelsUnlocked = 1;

        StartCoroutine(SlideIn(true));

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