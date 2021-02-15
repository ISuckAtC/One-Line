using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool bouncy = false;
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.transform.parent != null && collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            if (bouncy == false)                    //Placeholder until we have individual layer for rubber
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            Camera.main.transform.parent = null;
            collision.gameObject.GetComponent<PlayerController>().Kill(collision.GetContact(0).point);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }   

    }
}
