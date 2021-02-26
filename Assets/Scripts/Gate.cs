using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IActivatable
{
    public float MoveLength, Speed;
    private bool open;
    private Vector2 origin;

    void Start()
    {
        origin = transform.position;
    }

    void FixedUpdate()
    {
        if (open)
        {
            if (transform.position.y != origin.y + MoveLength)
            {
                if (transform.position.y + Speed > origin.y + MoveLength)
                {
                    transform.position = new Vector3(transform.position.x, origin.y + MoveLength);
                }
                else transform.Translate(new Vector3(0, Speed, 0));
            }
        }
        else if (transform.position.y != origin.y)
        {
            if (transform.position.y - Speed < origin.y)
            {
                transform.position = new Vector3(transform.position.x, origin.y);
            }
            else transform.Translate(new Vector3(0, -Speed, 0));
        }
    }

    public void Activate()
    {
        open = !open;
    }
}
