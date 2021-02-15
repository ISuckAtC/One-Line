using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inkwell : MonoBehaviour
{
    public LineType lineType;
    public int Amount;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch(lineType)
        {
            case LineType.Normal:
                sr.sprite = GameControl.main.InkNormal;
                break;
            case LineType.Ice:
                sr.sprite = GameControl.main.InkIce;
                break;
            case LineType.Rubber:
                sr.sprite = GameControl.main.InkRubber;
                break;
            case LineType.Weight:
                sr.sprite = GameControl.main.InkWeight;
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameControl.main.ModInk(lineType, Amount);
            Destroy(gameObject);
        }
    }
}
