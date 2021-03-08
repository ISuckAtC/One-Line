using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Dialogue[] Dialogues;
    public float DialogueTranscisionSpeed;
    private GameObject DialogueBoxEnemy, DialogueBoxHero;
    private Text DialogueBoxEnemyText, DialogueBoxHeroText;
    private AudioSource audioSource;
    public void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        //DialogueBoxEnemy = UiControl.main.Dialogue
        //DialogueBoxHero = UiControl.main.Dialogue
        DialogueBoxEnemyText = DialogueBoxEnemy.transform.GetChild(0).GetComponent<Text>();
        DialogueBoxHeroText = DialogueBoxHero.transform.GetChild(0).GetComponent<Text>();
        
        foreach(Dialogue d in Dialogues) d.Load();
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine(Play());
            GetComponent<Collider2D>().enabled = false;
        }
    }
    IEnumerator Play()
    {
        for(int i = 0; i < Dialogues.Length; ++i)
        {
            Dialogues[i].Speak(Dialogues[i].Enemy ? DialogueBoxEnemyText : DialogueBoxHeroText, audioSource);
            yield return new WaitForSeconds(DialogueTranscisionSpeed);
        }
    }
}
