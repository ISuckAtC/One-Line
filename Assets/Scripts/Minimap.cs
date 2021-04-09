using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Vector2 Ratios;
    public Image Map;
    public GameObject PlayerDot;
    public Vector2 Offset;
    
    // Start is called before the first frame update
    void Start()
    {
        Offset = GameControl.main.Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = ((Vector2)GameControl.main.Player.transform.position - Offset) * Ratios;
        PlayerDot.transform.localPosition = pos;
    }
}
