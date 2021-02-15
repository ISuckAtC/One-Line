using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inkwell : MonoBehaviour
{
    public LineType lineType;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameControl.main.ModInk(lineType, 1);
            Destroy(gameObject);
        }
    }
}
