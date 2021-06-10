using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroUI : MonoBehaviour
{

    Vector2 mousePosition;
    public GameObject GameCursor;

    void Update()
    {
        
        mousePosition = Input.mousePosition;
        GameCursor.transform.position = mousePosition;

    }
}
