using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_SetAcitve_Trigger : MonoBehaviour
{
    public GameObject destroy;
    public GameObject setActive, setActive2;
    public bool yes;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(destroy);
            setActive.SetActive(true);
            setActive2.SetActive(true);
            Destroy(gameObject);
            yes = true;
        }
    }
}
