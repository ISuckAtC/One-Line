using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Color BarColor;
    public GameObject Confines, Bar;
    private RectTransform CRect, BRect;
    public float Max;
    [SerializeField]
    private float ProsentOfTotal, CurrentAmount;
    public LineType InkType;
    public GameControl gc;
    public Sprite BarSprite;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameControl.main;
        CRect = Confines.GetComponent<RectTransform>();
        BRect = Bar.GetComponent<RectTransform>();
        Bar.GetComponent<Image>().sprite = BarSprite;
        CurrentAmount = gc.Ink[(int)InkType];
        Bar.GetComponent<Image>().color = BarColor;

    }

    // Update is called once per frame
    public void UpdateInkBar()
    {

        CurrentAmount = gc.Ink[(int)InkType];

        ProsentOfTotal = CurrentAmount / Max;

        BRect.sizeDelta = new Vector2(CRect.rect.width * ProsentOfTotal, CRect.rect.height * 0.99f);

    }
}
