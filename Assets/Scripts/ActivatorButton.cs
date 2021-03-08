using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorButton : MonoBehaviour
{
    public GameObject[] Activatables;
    private bool active;
    private Collider2D collider2Df;
    public void Start()
    {
        collider2Df = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (active) return;
        active = true;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        foreach(GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log(collider2Df.GetContacts(new ContactPoint2D[0]));
        if (collider2Df.GetContacts(new ContactPoint2D[0]) == 0) 
        {
            
            active = false;
        }
    }
}
