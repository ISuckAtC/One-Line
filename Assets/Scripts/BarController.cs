﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{

    public GameObject Confines, Bar;
    private RectTransform CRect, BRect;
    public float Max;
    private float ProsentOfTotal, CurrentAmount;
    private Vector2 BarPos;
    public LineType InkType;
    private GameControl gc;
    public Sprite BarSprite;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameControl.main;
        CRect = Confines.GetComponent<RectTransform>();
        BRect = Bar.GetComponent<RectTransform>();
        Bar.GetComponent<Image>().sprite = BarSprite;
        CurrentAmount = gc.Ink[(int)InkType];

    }

    // Update is called once per frame
    void Update()
    {

        CurrentAmount = gc.Ink[(int)InkType];

        ProsentOfTotal = CurrentAmount / Max;

        BarPos = new Vector2(0, 0);

        BRect.rect.Set(BarPos.x, BarPos.y, CRect.rect.width * ProsentOfTotal * 0.99f, CRect.rect.height * 0.99f);

    }
}
