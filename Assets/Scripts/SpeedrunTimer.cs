using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{

    public float runTime;
    public int CurrentScene;

    void Start()
    {

        DontDestroyOnLoad(this);

    }

    void Update()
    {

        if(SceneManager.GetActiveScene().buildIndex != 0)
            runTime += Time.deltaTime;

    }

    public void SceneCheck()
    {

        CurrentScene = SceneManager.GetActiveScene().buildIndex;

    }

}
