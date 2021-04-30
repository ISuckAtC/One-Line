using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWall : MonoBehaviour, IActivatable
{
    bool go;

    void FixedUpdate()
    {
        if (go == true)
        {
            transform.Translate(Vector2.up * Time.deltaTime * 3);
        }
    }

    public void Activate()
    {
        Debug.Log(gameObject.name + " activated");
        go = !go;
    }
}
