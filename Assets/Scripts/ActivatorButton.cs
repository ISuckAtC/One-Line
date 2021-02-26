using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorButton : MonoBehaviour
{
    public GameObject[] Activatables;

    public void OnTriggerEnter2D(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        foreach(GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
        }
    }
}
