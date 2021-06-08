using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesSoundController : MonoBehaviour
{
    public AudioSource lineSource;

    public AudioClip bounceClip, meltingClip;

    private Line _line;
    
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<Line>();
        bounceClip = Resources.Load<AudioClip>("Sound/Lines/RubberLineBounce");
        meltingClip = Resources.Load<AudioClip>("Sound/Lines/IceMelting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playMeltingIce()
    {
        if (!lineSource.isPlaying)
        {
            lineSource.clip = meltingClip;
            lineSource.loop = true;
            lineSource.Play();
        }
    }
}
