using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Dialogue[] Dialogues;
    public float DialogueTranscisionSpeed;
    public float DialoguePopupSpeed;
    private GameObject DialogueBoxEnemy, DialogueBoxHero;
    private Animator EnemyDialogueAnim, HeroDialogueAnim;
    private Text DialogueBoxEnemyText, DialogueBoxHeroText;
    private AudioSource audioSource;
    public void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        DialogueBoxEnemy = UiControl.main.DialogueBoxEnemy;
        DialogueBoxHero = UiControl.main.DialogueBoxHero;
        EnemyDialogueAnim = DialogueBoxEnemy.GetComponent<Animator>();
        HeroDialogueAnim = DialogueBoxHero.GetComponent<Animator>();
        DialogueBoxEnemyText = DialogueBoxEnemy.transform.GetChild(0).GetComponent<Text>();
        DialogueBoxHeroText = DialogueBoxHero.transform.GetChild(0).GetComponent<Text>();

        for (int i = 0; i < Dialogues.Length; ++i) Dialogues[i].Load();
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
            DialogueBoxEnemyText.text = "";
            DialogueBoxHeroText.text = "";
            if (Dialogues[i].Enemy)
            {
                EnemyDialogueAnim.ResetTrigger("MoveOut");
                EnemyDialogueAnim.SetTrigger("MoveIn");
                yield return new WaitForSeconds(DialoguePopupSpeed);
                yield return Dialogues[i].Speak(DialogueBoxEnemyText, audioSource);
                while(!Input.anyKey) yield return new WaitForFixedUpdate();
                EnemyDialogueAnim.ResetTrigger("MoveIn");
                EnemyDialogueAnim.SetTrigger("MoveOut");
                yield return new WaitForSeconds(DialogueTranscisionSpeed);
            }
            else
            {
                HeroDialogueAnim.ResetTrigger("MoveOut");
                HeroDialogueAnim.SetTrigger("MoveIn");
                yield return new WaitForSeconds(DialoguePopupSpeed);
                yield return Dialogues[i].Speak(DialogueBoxHeroText, audioSource);
                while(!Input.anyKey) yield return new WaitForFixedUpdate();
                HeroDialogueAnim.ResetTrigger("MoveIn");
                HeroDialogueAnim.SetTrigger("MoveOut");
                yield return new WaitForSeconds(DialogueTranscisionSpeed);
            }
        }
    }
}
