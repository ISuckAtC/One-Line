using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{

    public float runTime;

    void Start()
    {

        DontDestroyOnLoad(this);

    }

    void Update()
    {

        runTime += Time.deltaTime;

    }

    void Awake()
    {

        Debug.Log("Current scene: " + SceneManager.GetActiveScene().buildIndex);

    }

}
