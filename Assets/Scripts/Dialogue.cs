using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Dialogue
{
    const string vocals = "aeiouy";
    [TextArea] public string text;
    private AudioClip[] audios;
    public bool UseOwnSettings;
    public float pitchShift, NormalWait, SpaceWait, CommaWait, PeriodWait;
    public bool Enemy;
    public void Load()
    {
        audios = Resources.LoadAll<AudioClip>("Sound/HeroDialogue");
        if (!UseOwnSettings)
        {
            pitchShift = GameControl.main.DialoguePrimitive.pitchShift;
            NormalWait = GameControl.main.DialoguePrimitive.NormalWait;
            SpaceWait = GameControl.main.DialoguePrimitive.SpaceWait;
            CommaWait = GameControl.main.DialoguePrimitive.CommaWait;
            PeriodWait = GameControl.main.DialoguePrimitive.PeriodWait;
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
                    yield return new WaitForSecondsRealtime((float)customWait / 10f);
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
                    yield return new WaitForSecondsRealtime(PeriodWait);
                    break;
                case '!':
                    yield return new WaitForSecondsRealtime(PeriodWait);
                    break;
                case '?':
                    yield return new WaitForSecondsRealtime(PeriodWait);
                    break;
                case ',':
                    yield return new WaitForSecondsRealtime(CommaWait);
                    break;
                case ' ':
                    yield return new WaitForSecondsRealtime(SpaceWait);
                    break;
                default:
                    yield return new WaitForSecondsRealtime(NormalWait);
                    break;
            }
        }
    }
}
