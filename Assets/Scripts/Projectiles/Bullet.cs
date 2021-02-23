﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour
{
    public bool bouncy = false;
    public bool friendlyFire;
    public float LineDestroyRadius;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
    }

    void FixedUpdate()
    {
        transform.up = rb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.transform.parent != null && collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            Transform p = collision.transform.parent;
            if (bouncy == false && p.GetComponent<Line>().LineType != LineType.Rubber)                    //Placeholder until we have individual layer for rubber
            {
                List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, LineDestroyRadius).ToList();
                hits = hits.Where(x => x.transform.parent == p).ToList();
                foreach(Collider2D hit in hits) Destroy(hit.gameObject);
                Destroy(gameObject);
            } else friendlyFire = true;
        }

        if (collision.gameObject.tag == "Player")
        {
            Camera.main.transform.parent = null;
            collision.gameObject.GetComponent<PlayerController>().Kill(collision.GetContact(0).point);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && friendlyFire)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }   

    }
}