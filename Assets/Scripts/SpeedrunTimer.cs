using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
