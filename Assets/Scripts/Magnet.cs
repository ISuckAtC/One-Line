using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    public float magnetPower;
    [SerializeField]
    private List<GameObject> Lines;
    private GameObject lastCheckedLine;

    private void Start()
    {

        lastCheckedLine = null;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col != null)
        {

            if (col.transform.parent.GetComponent<Rigidbody2D>() && col.transform.parent.tag == "Line")
            {

                Lines.Add(col.transform.parent.gameObject);

            }

        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col != null)
        {

            if (col.transform.parent.GetComponent<Rigidbody2D>() && col.transform.parent.tag == "Line")
            {

                Lines.Remove(col.transform.parent.gameObject);

            }

        }

    }

    private void Update()
    {
        
        foreach(GameObject line in Lines)
        {

            Vector2 posDif = transform.position - line.transform.position; Vector2 posDifNormal = posDif.normalized;
            line.transform.GetComponent<Rigidbody2D>().velocity = line.transform.GetComponent<Rigidbody2D>().velocity + (posDifNormal * magnetPower);
            lastCheckedLine = line;

        }

    }

}
