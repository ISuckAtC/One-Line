using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenuController : MonoBehaviour
{

    public GameObject[] LevelButtons;
    public bool ExcludeLevelNames;
    public string[] LevelNames;
    public bool ExcludeLevelImages;
    public Sprite[] LevelPreviewImage;
    public bool ExcludeLevelTexts;
    public string[] LevelPreviewText;
    public GameObject MainMenuCursor, PreviewPanel, ToggleButton, HardmodeLevelButton, HardmodeToggleButton, GoToLevelButton, NormalBG, HardmodeBG;
    private GameObject speedrunTimer;
    public Text PreviewName, PreviewText, GoToLevelButtonText, HardmodeLevelButtonText, PreviewBestTimeText, PreviewBestHardTimeText, RecentRunText, BestRunText, BestCollectiveTimeText;
    public InputField FramerateInputField;
    public Image PreviewImage, Background;
    private Vector3 mousePosition;
    private int levelNumSelected, hardmodeLevelsUnlocked;
    private float slideInCounter, slideOutCounter, speedrunTime, collectiveBestTime;
    public bool ExcludeLevelTimes;
    public float[] levelTimes;
    public float PreviewPanelSpeed, screenWidth;
    private bool inView, hardMode, hardModeToggle;
    public CreateLevelTime CLT;
    private int ScreenWidth, ScreenHeight, Framerate;
    public bool FullscreenToggle;
    private SettingsData settingsData;
    private GameData gameData;
    public Color DefaultColor, HardModeColor;
    public Text TotalTime;
    public Text MOTD;

    void Start()
    {

        if(GameObject.FindGameObjectWithTag("Timer"))
        {

            speedrunTimer = GameObject.FindGameObjectWithTag("Timer");
            speedrunTime = speedrunTimer.GetComponent<SpeedrunTimer>().runTime;
            
        }

        if(SaveAndLoad.LoadGameData() == null)
            CLT.CreateNewTimes(false);

        if(SaveAndLoad.LoadSettingsData() == null)
            SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, Framerate, FullscreenToggle, false);

        gameData = SaveAndLoad.LoadGameData();

        if (!GameObject.Find("GameData")) 
        {
            GameObject global = new GameObject("Global Data");
            DontDestroyOnLoad(global);
            GlobalData g = global.AddComponent<GlobalData>();
            g.TotalRunTime = gameData.TotalRunTime;
        }

        settingsData = SaveAndLoad.LoadSettingsData();

        if(gameData.Times.Length < SceneManager.sceneCountInBuildSettings - 1)
        {

            CLT.CreateNewTimes(false);
            Debug.Log("Updating saved data...");
            gameData = SaveAndLoad.LoadGameData();

        }

        TotalTime.text = System.TimeSpan.FromSeconds(gameData.TotalRunTime).ToString(@"mm\:ss\.ff");

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
        hardmodeLevelsUnlocked = Mathf.Clamp(hardmodeLevelsUnlocked, 0, 60);
        Debug.Log("Hardmode levels unlocked: " + hardmodeLevelsUnlocked);
        Debug.Log("Levels Unlocked: " + gameData.LevelsUnlocked);

        if(gameData.LevelsUnlocked >= 27)
        {

            HardmodeToggleButton.GetComponent<Button>().interactable = true;
            for(int i = 0; i < LevelNames.Length; i++)
            {

                collectiveBestTime += levelTimes[i];

            }   

        }
        else
            HardmodeToggleButton.GetComponent<Button>().interactable = false;

        RecentRunText.text = "Last Run: " + gameData.RecentRun.ToString("00000.00");
        BestRunText.text = "Best Run: " + gameData.BestRun.ToString("00000.00");
        BestCollectiveTimeText.text = "Best Collective time: " + collectiveBestTime.ToString("00000.00");
        Background.color = DefaultColor;

        for(int i = 0; i < LevelButtons.Length; i++)
        {

            if(gameData.LevelsUnlocked >= i)
            {

                LevelButtons[i].GetComponent<Button>().interactable = true;

            }
            else
            {

                LevelButtons[i].GetComponent<Button>().interactable = false;
                
            }

        }

        MOTD.text = GetMOTD();
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

    public void ToggleHardMode()
    {

        hardModeToggle = !hardModeToggle;
        levelNumSelected = 1;
        
        if(hardModeToggle)
        {

            Background.color = HardModeColor;
            HardmodeLevelButton.SetActive(true);
            NormalBG.SetActive(false);
            HardmodeBG.SetActive(true);

        }
        else
        {

            Background.color = DefaultColor;
            HardmodeLevelButton.SetActive(false);
            NormalBG.SetActive(true);
            HardmodeBG.SetActive(false);

        }

        StartCoroutine(SlideIn(true));
        UpdateButtons();

    }

    public void UpdateButtons()
    {

        if(hardModeToggle)
        {

            for(int i = 0; i < LevelButtons.Length; i++)
            {

                LevelButtons[i].GetComponentInChildren<Text>().text = (i + LevelButtons.Length + 1).ToString();

                if(hardmodeLevelsUnlocked > i)
                {

                    LevelButtons[i].GetComponent<Button>().interactable = true;

                }
                else
                {

                    LevelButtons[i].GetComponent<Button>().interactable = false;
                
                }

            }
        
        }
        else
        {

            for(int i = 0; i < LevelButtons.Length; i++)
            {

                LevelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();

                if(gameData.LevelsUnlocked >= i)
                {

                    LevelButtons[i].GetComponent<Button>().interactable = true;

                }
                else
                {

                    LevelButtons[i].GetComponent<Button>().interactable = false;

                }

            }

        }

    }

    public void SetFramerate()
    {

        int framerate;
        if(int.TryParse(FramerateInputField.text, out framerate))
        {

            Framerate = int.Parse(FramerateInputField.text);
            FramerateInputField.text = "";

        }
        else
        {
            
            FramerateInputField.text = "Use numbers";

        }

    }

    public void SetWidth(int Width) => ScreenWidth = Width;

    public void SetHeight(int Height) => ScreenHeight = Height;

    public void FullscreenSwitch() => FullscreenToggle = ToggleButton.GetComponent<Toggle>().isOn;

    public void ApplySettings()
    {
        GetComponent<MenuSounds>().playApplySettingsSound();
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullscreenToggle);
        SaveAndLoad.SaveSettingsData(ScreenWidth, ScreenHeight, Framerate, FullscreenToggle, true);
        screenWidth = ScreenWidth;
        Application.targetFrameRate = Framerate;

    }

    public void GoToLevel()
    {

        if(speedrunTimer != null)
            if(speedrunTimer.GetComponent<SpeedrunTimer>().CurrentScene + 1 != levelNumSelected)
                Destroy(speedrunTimer);

        SceneManager.LoadScene(levelNumSelected);

    }

    public void GoToHardLevel()
    {

        SceneManager.LoadScene(levelNumSelected + LevelNames.Length);

    }

    private IEnumerator SlideIn(bool reset)
    {
        if(reset && levelNumSelected != 0)
        {
            GetComponent<MenuSounds>().playSlideSound();
            PreviewName.text = LevelNames[levelNumSelected-1];
            PreviewImage.sprite = LevelPreviewImage[levelNumSelected-1];
            PreviewText.text = LevelPreviewText[levelNumSelected-1];
            PreviewBestTimeText.text = "Best Time: " + levelTimes[levelNumSelected-1];
            PreviewBestHardTimeText.text = "Best Hardmode Time: " + levelTimes[levelNumSelected-1 + LevelNames.Length];
            GoToLevelButtonText.text = "Play Level: " + levelNumSelected;
            HardmodeLevelButtonText.text = "Play Level: " + (levelNumSelected + LevelButtons.Length);

            PreviewBestTimeText.gameObject.SetActive(!hardModeToggle);
            PreviewBestHardTimeText.gameObject.SetActive(hardModeToggle);

            HardmodeLevelButton.SetActive(hardModeToggle);
            GoToLevelButton.SetActive(!hardModeToggle);

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

    public void UnlockNormalLevels()
    {

        hardmodeLevelsUnlocked = 1;
        gameData.LevelsUnlocked = 28;
        HardmodeToggleButton.GetComponent<Button>().interactable = true;
        StartCoroutine(SlideIn(true));
        UpdateButtons();

    }

    public void UnlockAllLevels()
    {

        hardmodeLevelsUnlocked = 100;
        gameData.LevelsUnlocked = 100;
        HardmodeToggleButton.GetComponent<Button>().interactable = true;
        StartCoroutine(SlideIn(true));
        UpdateButtons();

    }

    public void EraseGameData()
    {
        GetComponent<MenuSounds>().playClearSaveSound();
        CLT.CreateNewTimes(false);
        hardmodeLevelsUnlocked = gameData.LevelsUnlocked - LevelNames.Length - 2;
        gameData = SaveAndLoad.LoadGameData();
        TotalTime.text = System.TimeSpan.FromSeconds(gameData.TotalRunTime).ToString(@"mm\:ss\.ff");
        UpdateButtons();
    }

    public void QuitGame() => Application.Quit();


    public string GetMOTD()
    {
        using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient())
        {
            Debug.Log("Attempting to connect");
            System.Threading.Tasks.Task.Run(() => client.Connect("18.117.229.1", 28000));
            int timeout = 0;
            while(!client.Connected)
            {
                if (timeout++ > 20)
                {
                    Debug.Log("Couldnt connect");
                    return null;
                }
                System.Threading.Thread.Sleep(50);
            }

            System.Net.Sockets.NetworkStream stream = client.GetStream();

            stream.Write(new byte[] {(byte)4}, 0, 1);
            byte[] buffer = new byte[256];
            stream.Read(buffer, 0, buffer.Length);
            int motdsize = (int)buffer[0];
            Debug.Log(motdsize);
            string motd = System.Text.Encoding.UTF8.GetString(buffer, 1, motdsize);
            stream.Dispose();
            return motd;
        }
    }

    
    public string[] GetLeaderBoards(bool hardmode)
    {
        using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient())
        {
            Debug.Log("Attempting to connect");
            System.Threading.Tasks.Task.Run(() => client.Connect("18.117.229.1", 28000));
            int timeout = 0;
            while(!client.Connected)
            {
                if (timeout++ > 20)
                {
                    Debug.Log("Couldnt connect");
                    return null;
                }
                System.Threading.Thread.Sleep(50);
            }

            System.Net.Sockets.NetworkStream stream = client.GetStream();
            stream.Write(new byte[] {(byte)(hardmode ? 1 : 0)}, 0, 1);
            byte[] buffer = new byte[4096];
            stream.Read(buffer, 0, 4096);
            int nameCount = System.BitConverter.ToInt32(buffer, 0);
            buffer = buffer.Skip(4).ToArray();
            string[] nameEntry = new string[nameCount];
            for (int i = 0; i < nameCount; ++i)
            {
                string name = System.Text.Encoding.UTF8.GetString(buffer, i * 36, 32);
                nameEntry[i] = (i + 1).ToString();
                nameEntry[i] += ": ";
                nameEntry[i] += name.Remove(name.IndexOf('\0'));
                nameEntry[i] += " - ";
                nameEntry[i] += System.TimeSpan.FromSeconds(System.BitConverter.ToSingle(buffer, (i * 36) + 32)).ToString(@"mm\:ss\.ff");
            }
            Debug.Log("Successfully got leaderboards");
            stream.Dispose();
            return nameEntry;
        }
    }
}