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
    public GameObject MainMenuCursor;
    public Text PreviewName, PreviewText, GoToLevelButtonText;
    public Image PreviewImage;
    private Vector3 mousePosition;
    private int levelNumSelected;

    void Update()
    {

        mousePosition = Input.mousePosition;
        MainMenuCursor.transform.position = mousePosition;

    }

    public void LevelPreview(int lvlNum)
    {

        levelNumSelected = lvlNum;

        PreviewName.text = LevelNames[lvlNum-1];
        PreviewImage.sprite = LevelPreviewImage[lvlNum-1];
        PreviewText.text = LevelPreviewText[lvlNum-1];
        GoToLevelButtonText.text = "Play Level: " + lvlNum;

    }

    public void GoToLevel()
    {

        SceneManager.LoadScene(levelNumSelected);

    }

}