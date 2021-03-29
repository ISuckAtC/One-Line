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
    public GameObject MainMenuCursor, PreviewPanel;
    public Text PreviewName, PreviewText, GoToLevelButtonText;
    public Image PreviewImage;
    private Vector3 mousePosition;
    private int levelNumSelected;
    public float slideInCounter, slideOutCounter, screenWidth;
    public bool inView;

    void Start()
    {

        screenWidth = gameObject.GetComponent<Canvas>().GetComponent<RectTransform>().rect.width;
        Cursor.visible = false;

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
            GoToLevelButtonText.text = "Play Level: " + levelNumSelected;

            slideInCounter = -1;
            inView = false;

        }

        if(slideInCounter < 1)
        {

            slideInCounter += Time.deltaTime * 6;
            slideInCounter = Mathf.Clamp(slideInCounter, -1, 1);
            PreviewPanel.transform.position = new Vector2(screenWidth + (-250 * slideInCounter), PreviewPanel.transform.position.y);
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

        if(reset) slideOutCounter = 1;

        if(slideOutCounter > -1)
        {

            slideOutCounter -= Time.deltaTime * 6;
            slideOutCounter = Mathf.Clamp(slideOutCounter, -1, 1);
            PreviewPanel.transform.position = new Vector2(screenWidth + (-250 * slideOutCounter), PreviewPanel.transform.position.y);
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