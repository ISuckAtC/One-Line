using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.transform.parent != null && collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Camera.main.transform.parent = null;
            collision.gameObject.GetComponent<PlayerController>().Kill(collision.GetContact(0).point);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        
    }
}
