using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivatorButton : MonoBehaviour
{
    public GameObject[] Activatables;
    private bool active;
    private Collider2D collider2Df;
    public bool Toggle = true;
    public bool SingleUse;
    public bool PlayerInteractable;
    public void Start()
    {
        collider2Df = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (active) return;
        if (Toggle) active = true;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (!PlayerInteractable && col.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        foreach(GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (SingleUse) return;
        Debug.Log(collider2Df.GetContacts(new ContactPoint2D[0]));
        List<ContactPoint2D> contactPoints = new List<ContactPoint2D>();
        collider2Df.GetContacts(contactPoints);
        if (contactPoints.Where(x => x.collider.gameObject.layer != LayerMask.NameToLayer("Ground") && (PlayerInteractable || x.collider.gameObject.layer != LayerMask.NameToLayer("Player"))).Count() == 0) 
        {
            if (Toggle) active = false;
            else foreach(GameObject activatable in Activatables)
            {
                activatable.GetComponent<IActivatable>().Activate();
            }
        }
    }
}
