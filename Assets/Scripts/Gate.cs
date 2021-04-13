using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IActivatable
{
    public float MoveLength, Speed;
    public bool Open;
    public bool Horizontal;
    private Vector2 origin;

    void Start()
    {
        origin = transform.position;
    }

    void FixedUpdate()
    {
        if (Open)
        {
            if (Vector2.Distance(origin, transform.position) < MoveLength)
            {
                transform.position = Vector2.MoveTowards(transform.position, origin + new Vector2(Horizontal ? MoveLength : 0, Horizontal ? 0 : MoveLength), Speed);
            }
        }
        else if ((Vector2)transform.position != origin)
        {
            if (Vector2.Distance(origin, transform.position) > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, origin, Speed);
            }
        }
    }

    public void Activate()
    {
        Debug.Log(gameObject.name + " activated");
        Open = !Open;
    }
}
