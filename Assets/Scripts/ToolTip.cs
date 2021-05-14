using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : Selectable
{
    public string HoverText;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        UiControl.main.CursorToolTip.SetActive(true);
        UiControl.main.CursorToolTip.transform.GetChild(0).GetComponent<Text>().text = HoverText;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        UiControl.main.CursorToolTip.transform.GetChild(0).GetComponent<Text>().text = "";
        UiControl.main.CursorToolTip.SetActive(false);
    }
}
