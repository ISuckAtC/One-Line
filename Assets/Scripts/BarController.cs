using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Color BarColor;
    public GameObject Confines, Bar, TextObject;
    private RectTransform CRect, BRect;
    private Text text;
    public float Max;
    [SerializeField]
    private float ProsentOfTotal, CurrentAmount, inkDisplayValue, inkFade;
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
        text = TextObject.GetComponent<Text>();
        text.color = new Color(0.5f, 0.6f, 0.1f, 0);

    }

    public void TempBarUpdate(float inkAmount)
    {
        
        CurrentAmount = gc.Ink[(int)InkType];
        ProsentOfTotal = CurrentAmount / Max - inkAmount;
        ProsentOfTotal = Mathf.Clamp(ProsentOfTotal, 0, 1);
        BRect.sizeDelta = new Vector2(CRect.rect.width * ProsentOfTotal, CRect.rect.height * 0.99f);

        inkDisplayValue = ProsentOfTotal * Max;

    }

    public void UpdateInkBar()
    {

        if(gc != null)
        {
            
            CurrentAmount = gc.Ink[(int)InkType];
            ProsentOfTotal = CurrentAmount / Max;
            ProsentOfTotal = Mathf.Clamp(ProsentOfTotal, 0, 1);
            BRect.sizeDelta = new Vector2(CRect.rect.width * ProsentOfTotal, CRect.rect.height * 0.99f);

            inkDisplayValue = ProsentOfTotal * Max;

        }

    }

    public void StartFade() => StartCoroutine(InkValueFade());

    public IEnumerator InkValueFade()
    {

        inkFade -= Time.unscaledDeltaTime;
        text.color = new Color(0.5f, 0.6f, 0.1f, inkFade);


        yield return new WaitForEndOfFrame();
        if(inkFade > 0)
            StartCoroutine(InkValueFade());

    }

    public void UpdateInkValue()
    {
        
        text.text = inkDisplayValue.ToString();
        inkFade = 1;
        text.color = new Color(0.5f, 0.6f, 0.1f, 1);

    }

}
