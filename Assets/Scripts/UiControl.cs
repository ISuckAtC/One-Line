using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    [SerializeField]
    private GameObject PauseGameUi;
    [SerializeField]
    private GameObject InGameUi;
    private bool PauseGameUiOnOff;
    [SerializeField]
    private GameControl gc;
    [SerializeField]
    private Text coinsText;

    // Start is called before the first frame update
    void Start()
    {

        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        PauseGameUiOnOff = false;
        PauseGameUi = GameObject.Find("PauseGameUi");
        InGameUi = GameObject.Find("InGameUi");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGameUiOnOff = !PauseGameUiOnOff;

        PauseGameUi.SetActive(PauseGameUiOnOff);
        InGameUi.SetActive(!PauseGameUiOnOff);

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
