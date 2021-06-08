using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Heat : MonoBehaviour
{
    public float HeatInterval;
    public int HeatAmount;
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
        heatArea.OverlapCollider((new ContactFilter2D()).NoFilter(), touching);
        Line buf = null;
        GameObject[] icePieces = System.Array.ConvertAll(touching.FindAll(x => x.transform.parent != null && x.transform.parent.TryGetComponent<Line>(out buf) && buf.LineType == LineType.Ice).ToArray(), x => x.gameObject);
        if (icePieces.Length > 0) 
        {
            icePieces[0].GetComponentInParent<LinesSoundController>().playMeltingIce();
            foreach (GameObject piece in icePieces)
            {
                int index = int.Parse(piece.name.Substring(2));
                piece.transform.parent.GetComponent<Line>().Pieces[index].HeatUp(HeatAmount);
            }
        }
    }
}
