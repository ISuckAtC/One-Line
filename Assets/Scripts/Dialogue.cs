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
            DisplayText.text = text.Substring(0, i + 1);
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
