using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneOnTheFly : MonoBehaviour, IActivatable
{
    public Dialogue[] Dialogues;
    public float DialogueTranscisionSpeed;
    public float DialoguePopupSpeed;
    private AudioSource audioSource;
    public GameObject DialogueBox;
    public Text DialogueText;
    public Vector2 DialogueOffset;

    private int playIndex;
    private bool playing;

    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playing)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(Dialogues[playIndex].OnTheFlyAttach.transform.position);
            pos.z = DialogueBox.transform.position.z;
            pos += (Vector3)DialogueOffset;
            DialogueBox.transform.position = pos;
        }
    }

    public void Activate()
    {
        foreach (Dialogue d in Dialogues) d.Load();
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        playing = true;
        DialogueBox.SetActive(true);
        for (; playIndex < Dialogues.Length; ++playIndex)
        {
            DialogueText.text = "";

            yield return Dialogues[playIndex].Speak(DialogueText, DialogueBox.transform.GetChild(0) as RectTransform, audioSource);

            yield return new WaitForSecondsRealtime(Dialogues[playIndex].PostPlayWait);
        }
        DialogueBox.SetActive(false);
        playing = false;
    }
}
