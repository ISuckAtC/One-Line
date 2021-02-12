using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiControl : MonoBehaviour
{

    private GameObject inGameUi;
    private bool inGameUiOnOff;

    // Start is called before the first frame update
    void Start()
    {

        inGameUiOnOff = false;
        inGameUi = GameObject.Find("inGameUi");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            inGameUiOnOff = !inGameUiOnOff;

        inGameUi.SetActive(inGameUiOnOff);

    }

    public void MainMenuButton()
    {

        SceneManager.LoadScene(0);

    }

    public void QuitButton()
    {

        Application.Quit();

    }

    public void LevelSelect(int lvlToLoad)
    {

        SceneManager.LoadScene(lvlToLoad);

    }

}
