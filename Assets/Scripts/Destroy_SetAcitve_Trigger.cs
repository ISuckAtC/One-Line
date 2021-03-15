using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_SetAcitve_Trigger : MonoBehaviour
{
    public GameControl destroy;
    public GameObject setActive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(destroy);
        setActive.SetActive(true);
        Destroy(gameObject);
    }
}
