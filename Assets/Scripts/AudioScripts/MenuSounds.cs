using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuSounds : MonoBehaviour
{
    public AudioClip buttonClick;
    public AudioClip slideSoundClip, applySettingsClip, clearSaveClip;
    public AudioSource mainMenuSource;
    
    public List<Button> allMenuButtons = new List<Button>();
    
    // Start is called before the first frame update
    void Start()
    {
        allMenuButtons = Resources.FindObjectsOfTypeAll<Button>().ToList();

        foreach (Button butt in allMenuButtons)
        {
            butt.onClick.AddListener(playButtonClick);
        }
    }

    private void playButtonClick()
    {
        mainMenuSource.pitch = Random.Range(0.75f, 1.25f);
        mainMenuSource.PlayOneShot(buttonClick);
    }

    public void playSlideSound()
    {
        mainMenuSource.pitch = Random.Range(0.8f, 1.2f);
        mainMenuSource.PlayOneShot(slideSoundClip);
    }

    public void playClearSaveSound()
    {
        mainMenuSource.PlayOneShot(clearSaveClip);
    }

    public void playApplySettingsSound()
    {
        mainMenuSource.PlayOneShot(applySettingsClip);
    }

    private IEnumerator playSounds(AudioClip clip, AudioSource source, int number)
    {
        for (int i = 0; i < number; i++)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
