using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.transform.parent.GetComponent<Rigidbody2D>() && col.transform.parent.tag == "Line") Debug.Log("yes:3");

    }

}
