using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuSounds : MonoBehaviour
{
    public AudioClip buttonClick;
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
}
