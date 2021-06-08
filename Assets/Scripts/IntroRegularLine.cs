using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRegularLine : MonoBehaviour
{
    public Animator playerNormal, playerIntro, bridgeIntro, impIntro, fireIntro;
    public GameObject bridge, fire, inGameCam, animCam;
    public GameObject belly;
    // Start is called before the first frame update
    void Start()
    {
        GameControl.main.InCutScene = true;
        UiControl.main.InGameUi.SetActive(false);
        belly.SetActive(true);
        playerNormal.enabled = false;
        StartCoroutine(BridgeDelay());
        StartCoroutine(FireDelay());
        StartCoroutine(Disapear());
        StartCoroutine(BackToNormal());
    }
    
    IEnumerator BridgeDelay()
    {
        yield return new WaitForSeconds(2);
        bridgeIntro.Play("Bridge");
    }
    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(6);
        fireIntro.Play("IntroFire");
    }
    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(7);
        bridge.SetActive(false);
        fire.SetActive(false);
    }
    IEnumerator BackToNormal()
    {
        yield return new WaitForSeconds(9);
        GameControl.main.InCutScene = false;
        UiControl.main.InGameUi.SetActive(true);
        playerIntro.enabled = false;
        playerNormal.enabled = true;
        inGameCam.SetActive(true);
        animCam.SetActive(false);
    }
}
