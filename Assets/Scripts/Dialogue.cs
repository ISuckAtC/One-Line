using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    const string vocals = "aeiouy";
    [TextArea] public string text;
    private AudioClip[] audios;
    public bool UseOwnSettings;
    public float pitchShift, NormalWait, SpaceWait, CommaWait, PeriodWait, WaitSpeedUp;
    [SerializeField] private float waitMult;
    public bool Enemy;
    public GameObject OnTheFlyAttach;
    public void Load()
    {
        audios = Resources.LoadAll<AudioClip>("Sound/HeroDialogue");
        waitMult = 1;
        if (!UseOwnSettings)
        {
            pitchShift = GameControl.main.DialoguePrimitive.pitchShift;
            NormalWait = GameControl.main.DialoguePrimitive.NormalWait;
            SpaceWait = GameControl.main.DialoguePrimitive.SpaceWait;
            CommaWait = GameControl.main.DialoguePrimitive.CommaWait;
            PeriodWait = GameControl.main.DialoguePrimitive.PeriodWait;
            WaitSpeedUp = GameControl.main.DialoguePrimitive.WaitSpeedUp;
        }
    }
    public void DecreaseWait()
    {
        waitMult = waitMult - (waitMult * (WaitSpeedUp / 100)) > 0 ? waitMult - (waitMult * (WaitSpeedUp / 100)) : 0;
    }
    public IEnumerator SpeedUp(CutScene cut, int i)
    {
        bool first = true;
        while(true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            Debug.Log(waitMult);
            cut.Dialogues[i].DecreaseWait();
            if (first)
            {
                first = false;
                WaitSpeedUp /= 10;
            }
        }
    }
    public IEnumerator Speak(Text DisplayText, AudioSource source)
    {
        bool prevVocal = false;
        
        for(int i = 0; i < text.Length; ++i)
        {
            if (text[i] == GameControl.main.CustomWaitDefCharacter)
            {
                int customWait;
                if (int.TryParse(text.Substring(i + 1, GameControl.main.CustomWaitDefDigits), out customWait))
                {
                    i += GameControl.main.CustomWaitDefDigits;
                    yield return new WaitForSecondsRealtime(((float)customWait / 10f) * waitMult);
                    continue;
                } else throw new System.ArgumentException("Number of digits in wait definition was lower than num defined in GameControl. Num defined in GC: [" + GameControl.main.CustomWaitDefDigits + "]");
            }
            DisplayText.text += text[i];
            if (vocals.Contains(text[i].ToString()))
            {
                if (!prevVocal && audios.Length > 0)
                {
                    source.pitch = Random.Range(1 - pitchShift, 1 + pitchShift);
                    source.clip = audios[Random.Range(0, audios.Length)];
                    source.Play();
                }
                prevVocal = true;
            } else prevVocal = false;
            switch(text[i])
            {
                case '.':
                    yield return new WaitForSecondsRealtime(PeriodWait * waitMult);
                    break;
                case '!':
                    yield return new WaitForSecondsRealtime(PeriodWait * waitMult);
                    break;
                case '?':
                    yield return new WaitForSecondsRealtime(PeriodWait * waitMult);
                    break;
                case ',':
                    yield return new WaitForSecondsRealtime(CommaWait * waitMult);
                    break;
                case ' ':
                    yield return new WaitForSecondsRealtime(SpaceWait * waitMult);
                    break;
                default:
                    yield return new WaitForSecondsRealtime(NormalWait * waitMult);
                    break;
            }
        }
    }
}
