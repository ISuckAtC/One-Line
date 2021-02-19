using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    public float HeatInterval;
    private BoxCollider2D heatArea;
    // Start is called before the first frame update
    void Start()
    {
        heatArea = transform.GetChild(0).GetComponent<BoxCollider2D>();
        InvokeRepeating(nameof(HeatWave), 0, HeatInterval);
    }

    public void HeatWave()
    {
        List<Collider2D> touching = new List<Collider2D>();
        ContactFilter2D a = new ContactFilter2D();
        heatArea.OverlapCollider((new ContactFilter2D()).NoFilter(), touching);
        Line buf;
        Debug.Log(touching.Count);
        Collider2D[] ice = touching.FindAll(x => x.transform.parent != null && x.transform.parent.TryGetComponent<Line>(out buf) && buf.LineType == LineType.Ice).ToArray();
        foreach (Collider2D icePiece in ice) Destroy(icePiece.gameObject);
    }
}
