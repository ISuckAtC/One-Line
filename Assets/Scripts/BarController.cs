using System.Collections;
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
    private LineType InkType;

    // Start is called before the first frame update
    void Start()
    {

        CRect = Confines.GetComponent<RectTransform>();
        BRect = Bar.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {

        BarPos = new Vector2(-CRect.rect.width * (1 - ProsentOfTotal) / 2 + CRect.position.x, CRect.position.y);

        ProsentOfTotal = CurrentAmount / Max;

        BRect.sizeDelta = new Vector2(CRect.rect.width * ProsentOfTotal, 20);

        BRect.position = BarPos;

    }
}
