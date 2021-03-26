using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour, IActivatable
{
    public Dialogue[] Dialogues;
    public float DialogueTranscisionSpeed;
    public float DialoguePopupSpeed;
    private GameObject DialogueBoxEnemy, DialogueBoxHero;
    private Animator EnemyDialogueAnim, HeroDialogueAnim;
    private Text DialogueBoxEnemyText, DialogueBoxHeroText;
    private AudioSource audioSource;
    public int SecondsHoldToSkip;
    private bool playing;
    [SerializeField] private float skip;
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
    public void Activate()
    {
        StartCoroutine(Play());
    }
    public void Update()
    {
        if (playing)
        {
            if (Input.GetMouseButton(1)) skip += Time.unscaledDeltaTime;
            else if (skip < SecondsHoldToSkip) skip = 0;

            /*if (skip > SecondsHoldToSkip)
            {
                StopCoroutine(Play());
                EnemyDialogueAnim.ResetTrigger("MoveOut");
                EnemyDialogueAnim.ResetTrigger("MoveIn");
                EnemyDialogueAnim.SetTrigger("MoveOut");
                HeroDialogueAnim.ResetTrigger("MoveOut");
                HeroDialogueAnim.ResetTrigger("MoveIn");
                HeroDialogueAnim.SetTrigger("MoveOut");
                Time.timeScale = 1;
                GameControl.main.InCutScene = false;
            }*/
        }
    }
    IEnumerator Play()
    {
        playing = true;
        Time.timeScale = 0;
        GameControl.main.InCutScene = true;
        for (int i = 0; i < Dialogues.Length; ++i)
        {
            if (skip > SecondsHoldToSkip) break;
            DialogueBoxEnemyText.text = "";
            DialogueBoxHeroText.text = "";
            if (Dialogues[i].Enemy)
            {
                EnemyDialogueAnim.ResetTrigger("MoveOut");
                EnemyDialogueAnim.SetTrigger("MoveIn");
                yield return new WaitForSecondsRealtime(DialoguePopupSpeed);
                StartCoroutine(Dialogues[i].SpeedUp(this, i));
                yield return Dialogues[i].Speak(DialogueBoxEnemyText, audioSource, this, i);
                StopCoroutine(Dialogues[i].SpeedUp(this, i));
                yield return new WaitWhile(() => !Input.anyKey);
                EnemyDialogueAnim.ResetTrigger("MoveIn");
                EnemyDialogueAnim.SetTrigger("MoveOut");
                yield return new WaitForSecondsRealtime(DialogueTranscisionSpeed);
            }
            else
            {
                HeroDialogueAnim.ResetTrigger("MoveOut");
                HeroDialogueAnim.SetTrigger("MoveIn");
                yield return new WaitForSecondsRealtime(DialoguePopupSpeed);
                StartCoroutine(Dialogues[i].SpeedUp(this, i));
                yield return Dialogues[i].Speak(DialogueBoxHeroText, audioSource, this, i);
                StopCoroutine(Dialogues[i].SpeedUp(this, i));
                yield return new WaitWhile(() => !Input.anyKey);
                HeroDialogueAnim.ResetTrigger("MoveIn");
                HeroDialogueAnim.SetTrigger("MoveOut");
                yield return new WaitForSecondsRealtime(DialogueTranscisionSpeed);
            }
        }
        Time.timeScale = 1;
        GameControl.main.InCutScene = false;
        playing = false;
    }
}
