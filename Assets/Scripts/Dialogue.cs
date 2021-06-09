using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    const string vocals = "aeiouy";
    [TextArea] public string text;
    public string AudioLocation;
    private AudioClip[] audios;
    public bool UseOwnSettings;
    public float pitchShift, NormalWait, SpaceWait, CommaWait, PeriodWait, WaitSpeedUp;
    [SerializeField] private float waitMult;
    public bool Enemy;
    public GameObject OnTheFlyAttach;
    public float PostPlayWait;
    public void Load()
    {
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
        else
        {
            if (pitchShift <= 0) pitchShift = GameControl.main.DialoguePrimitive.pitchShift;
            if (NormalWait <= 0) NormalWait = GameControl.main.DialoguePrimitive.NormalWait;
            if (SpaceWait <= 0) SpaceWait = GameControl.main.DialoguePrimitive.SpaceWait;
            if (CommaWait <= 0) CommaWait = GameControl.main.DialoguePrimitive.CommaWait;
            if (PeriodWait <= 0) PeriodWait = GameControl.main.DialoguePrimitive.PeriodWait;
            if (WaitSpeedUp <= 0) WaitSpeedUp = GameControl.main.DialoguePrimitive.WaitSpeedUp;
        }
        if (AudioLocation == string.Empty) AudioLocation = "HeroDialogue";
        audios = Resources.LoadAll<AudioClip>("Sound/" + AudioLocation);
    }
    public void DecreaseWait()
    {
        waitMult = waitMult - (waitMult * (WaitSpeedUp / 100)) > 0 ? waitMult - (waitMult * (WaitSpeedUp / 100)) : 0;
    }
    public IEnumerator SpeedUp(CutScene cut, int i)
    {
        bool first = true;
        while (true)
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
    public IEnumerator Shake(RectTransform box, Vector2 bounds, float speed, int points)
    {
        Vector3[] pointPositions = new Vector3[points];
        Vector3 startPosition = box.position;
        for (int i = 0; i < points; ++i)
        {
            pointPositions[i] = new Vector3(Random.Range(box.position.x - bounds.x, box.position.x + bounds.x), Random.Range(box.position.y - bounds.y, box.position.y + bounds.y), box.position.z);
        }

        for (int i = 0; i < points; ++i)
        {
            Debug.Log(i);
            while (Vector3.Distance(pointPositions[i], box.position) > 0)
            {
                box.position = Vector3.MoveTowards(box.position, pointPositions[i], speed);
                yield return new WaitForSeconds(1f / 60f);
            }
        }
        while (Vector3.Distance(startPosition, box.position) > 0)
        {
            box.position = Vector3.MoveTowards(box.position, startPosition, speed);
            yield return new WaitForSeconds(1f / 60f);
        }
    }
    public IEnumerator Speak(Text DisplayText, RectTransform box, AudioSource source)
    {
        bool prevVocal = false;
        bool forceSound = false;

        for (int i = 0; i < text.Length; ++i)
        {
            if (text[i] == GameControl.main.CustomForceSoundDefCharacter)
            {
                forceSound = true;
                i++;
            }
            if (text[i] == GameControl.main.CustomShakeDefCharacter)
            {
                GameControl.main.StartCoroutine(Shake(box, new Vector2(20, 20), 100, 20));
                continue;
            }
            if (text[i] == GameControl.main.CustomWaitDefCharacter)
            {
                int customWait;
                if (int.TryParse(text.Substring(i + 1, GameControl.main.CustomWaitDefDigits), out customWait))
                {
                    i += GameControl.main.CustomWaitDefDigits;
                    yield return new WaitForSecondsRealtime(((float)customWait / 10f) * waitMult);
                    continue;
                }
                else throw new System.ArgumentException("Number of digits in wait definition was lower than num defined in GameControl. Num defined in GC: [" + GameControl.main.CustomWaitDefDigits + "]");
            }
            DisplayText.text += text[i];

            if (vocals.Contains(text[i].ToString()))
            {
                if (!prevVocal)
                {
                    source.pitch = Random.Range(1 - pitchShift, 1 + pitchShift);
                    source.clip = audios[Random.Range(0, audios.Length)];
                    source.Play();
                }
                prevVocal = true;
            }
            else
            {
                prevVocal = false;
                if (forceSound)
                {
                    forceSound = false;
                    source.pitch = Random.Range(1 - pitchShift, 1 + pitchShift);
                    source.clip = audios[Random.Range(0, audios.Length)];
                    source.Play();
                }
            }
            switch (text[i])
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
