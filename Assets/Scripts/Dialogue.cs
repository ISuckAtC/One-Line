using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Dialogue
{
    const string vocals = "aeiouy";
    public string text;
    public AudioClip[] audios;
    public float pitchShift, NormalWait, SpaceWait, CommaWait, PeriodWait;
    public AudioSource source;
    public Text DisplayText;
    public IEnumerator Speak()
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
                    yield return new WaitForSeconds(customWait);
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
                    yield return new WaitForSeconds(PeriodWait);
                    break;
                case ',':
                    yield return new WaitForSeconds(CommaWait);
                    break;
                case ' ':
                    yield return new WaitForSeconds(SpaceWait);
                    break;
                default:
                    yield return new WaitForSeconds(NormalWait);
                    break;
            }
        }
    }
}
