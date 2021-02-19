using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    private GameObject PauseGameUi;
    private GameObject InGameUi;
    public bool PauseGameUiOnOff;
    private GameControl gc;
    private Text coinsText;
    private GameObject FinishUi;

    // Start is called before the first frame update
    void Start()
    {

        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        FinishUi = GameObject.Find("FinishUi");
        PauseGameUiOnOff = false;
        PauseGameUi = GameObject.Find("PauseGameUi");
        InGameUi = GameObject.Find("InGameUi");
        FinishUi.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGameUiOnOff = !PauseGameUiOnOff;

        if(InGameUi)
        {

            PauseGameUi.SetActive(PauseGameUiOnOff);
            InGameUi.SetActive(!PauseGameUiOnOff);
            coinsText.GetComponent<Text>().text = "coins: " + gc.Coins;

        }
        
    }

    public void MainMenuButton() => SceneManager.LoadScene(0);

    public void QuitButton() => Application.Quit();

    public void LevelSelect(int lvlToLoad) => SceneManager.LoadScene(lvlToLoad);

    public void LevelFinish() => FinishUi.SetActive(true);

    public void NextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void ReplayLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}
