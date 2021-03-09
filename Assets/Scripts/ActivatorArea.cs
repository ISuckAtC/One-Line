using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorArea : MonoBehaviour
{
    public GameObject[] Activatables;
    public bool OneTime;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) foreach(GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
            if (OneTime) GetComponent<Collider2D>().enabled = false;
        }
        
    }
}
