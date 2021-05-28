using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    string currentTime;
    public GameObject cam;
    public UnityEngine.Video.VideoClip vidSource;
    public GameObject UICanvas;
    void Start()
    {
        UICanvas.SetActive(false);
        GameControl.main.InCutScene = true;
        var videoPlayer = cam.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.clip = vidSource;
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
        /*
         * video = video.find;
        Play the video.

        if (input.getkeydown(keycode.any))
        {
        Show esc to skip
        currentTime = time;
        if (input.getkeydown(Keycode.any)&& currentTime == time)
        {
            Loadscene(Buildindex"1")
        }else
        {
        Don't show esc to skip
        }

        if (video.end ))
        {
            Loadscene(Buildindex"1")
        }
         */
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        GameControl.main.InCutScene = false;
        cam.GetComponent<VideoPlayer>().Stop();
        UICanvas.SetActive(true);
        Destroy(gameObject);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameControl.main.InCutScene = false;
            cam.GetComponent<VideoPlayer>().Stop();
            UICanvas.SetActive(true);
            Destroy(gameObject);
        }
        //string time = DateTime.Now.ToString("t");
    }
}
