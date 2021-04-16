using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBGLoader : MonoBehaviour
{

    public Sprite[] sprites;
    public bool BossLevel;

    void Start()
    {
        if(BossLevel)
            sprites = Resources.LoadAll("BossBackGrounds", typeof (Sprite)).Cast<Sprite>().ToArray();
        else
            sprites = Resources.LoadAll("BackGrounds", typeof (Sprite)).Cast<Sprite>().ToArray();
        Debug.Log(sprites.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

}
