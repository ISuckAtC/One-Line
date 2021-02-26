using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{

    public GameObject Confines, Bar;
    public RectTransform CRect, BRect;
    public float Max, CurrentAmount;
    [SerializeField]
    public float ProsentOfTotal;
    public Vector2 BarPos;

    // Start is called before the first frame update
    void Start()
    {

        Confines = GameObject.Find("Confines");
        Bar = GameObject.Find("Bar");

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
