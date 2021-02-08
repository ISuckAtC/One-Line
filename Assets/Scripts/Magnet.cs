using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    public float magnetPower;

    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.transform.parent.GetComponent<Rigidbody2D>() && col.transform.parent.tag == "Line")
        {

            Vector2 posDif = transform.position - col.transform.parent.position; Vector2 posDifNormal = posDif.normalized; 
            col.transform.parent.GetComponent<Rigidbody2D>().velocity = col.transform.parent.GetComponent<Rigidbody2D>().velocity + (posDifNormal * magnetPower);

        }

    }

}
