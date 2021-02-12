using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    private GameObject inGameUi;
    private bool inGameUiOnOff;
    private GameControl gc;
    private Text coinsText;

    // Start is called before the first frame update
    void Start()
    {

        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        inGameUiOnOff = false;
        inGameUi = GameObject.Find("inGameUi");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            inGameUiOnOff = !inGameUiOnOff;

        inGameUi.SetActive(inGameUiOnOff);

        coinsText.GetComponent<Text>().text = "coins: " + gc.Coins;

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
