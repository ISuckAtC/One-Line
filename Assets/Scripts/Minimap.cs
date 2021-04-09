using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public float Ratio;
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
        PlayerDot.transform.localPosition = ((Vector2)GameControl.main.Player.transform.position - Offset) * Ratio;
    }
}
