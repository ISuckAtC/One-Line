using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour
{
    public bool bouncy = false;
    public bool friendlyFire, DestroyGravLines;
    public float LineDestroyRadius;
    public float LifeTime = 30;
    private Rigidbody2D rb;
    private Collider2D collider2d;
    private void Start()
    {
        Destroy(gameObject, LifeTime);
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        transform.up = rb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            BulletSoundController _bsc = GetComponent<BulletSoundController>();
            _bsc.playDestroyClip();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, _bsc.destroyedClip.length);
        }

        if (collision.transform.parent != null && collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            Transform p = collision.transform.parent;
            Line l = p.GetComponent<Line>();
            l.Refund = false;
            if (bouncy == false && l.LineType != LineType.Rubber)                    //Placeholder until we have individual layer for rubber
            {
                List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, LineDestroyRadius).ToList();
                hits = hits.Where(x => x.transform.parent == p).ToList();
                foreach(Collider2D hit in hits) Destroy(hit.gameObject);
                Destroy(gameObject);
            }
            else
            {

                friendlyFire = true;
                gameObject.layer = LayerMask.NameToLayer("ProjectileActive");
                List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, LineDestroyRadius).ToList();
                hits = hits.Where(x => x.transform.parent == p).ToList();
                foreach(Collider2D hit in hits) Destroy(hit.gameObject);

            }
        }
        else if(DestroyGravLines && collision.gameObject.layer == LayerMask.NameToLayer("Line") && collision.gameObject.GetComponent<Rigidbody2D>())
        {

            List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, LineDestroyRadius, 1 << LayerMask.NameToLayer("Line")).ToList();
            foreach (Collider2D hit in hits)Destroy(hit.gameObject);

            Destroy(gameObject);

        }

        if (collision.gameObject.tag == "Player")
        {
            Camera.main.transform.parent = null;
            collision.gameObject.GetComponent<PlayerController>().Kill(collision.GetContact(0).point);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && friendlyFire)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }   

    }
}
