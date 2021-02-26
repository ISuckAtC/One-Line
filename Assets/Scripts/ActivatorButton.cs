using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorButton : MonoBehaviour
{
    public GameObject[] Activatables;
    private bool active;
    private Collider2D collider;
    public void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (active) return;
        active = true;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        foreach(GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (collider.GetContacts(new ContactPoint2D[0]) == 0) active = false;
    }
}
