using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inkwell : MonoBehaviour
{
    public LineType lineType;
    public int Amount;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameControl.main.ModInk(lineType, Amount);
            Destroy(gameObject);
        }
    }
}
