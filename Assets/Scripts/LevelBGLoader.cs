using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBGLoader : MonoBehaviour
{

    public Sprite[] Backgrounds;
    public SpriteRenderer[] InkBackgrounds;
    public bool BossLevel;
    private float width, height;

    void Start()
    {

        width = gameObject.GetComponent<SpriteRenderer>().size.x;
        height = gameObject.GetComponent<SpriteRenderer>().size.y;

        if(BossLevel)
            Backgrounds = Resources.LoadAll("BossLevelBackgrounds", typeof (Sprite)).Cast<Sprite>().ToArray();
        else
            Backgrounds = Resources.LoadAll("LevelBackgrounds", typeof (Sprite)).Cast<Sprite>().ToArray();
        Debug.Log(Backgrounds.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = Backgrounds[Random.Range(0, Backgrounds.Length)];

        InkBackgrounds[0].sprite = Resources.Load<Sprite>("InkBackgrounds/Regular");
        InkBackgrounds[1].sprite = Resources.Load<Sprite>("InkBackgrounds/Ice");
        InkBackgrounds[2].sprite = Resources.Load<Sprite>("InkBackgrounds/Rubber");
        InkBackgrounds[3].sprite = Resources.Load<Sprite>("InkBackgrounds/Gravity");

        for(int i = 0; i < InkBackgrounds.Length; i++)
        {

            InkBackgrounds[i].size = new Vector2(width, height);

        }

    }

}
