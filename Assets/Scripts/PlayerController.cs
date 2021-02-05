using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float BumpForce, FallSpeed;
    private Rigidbody2D rb;
    private float defaultGravity, defaultDrag, lastSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        defaultDrag = rb.drag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FallSpeed);
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        if (capsule.GetContacts(colliders) > 0)
        {
            Line line = null;
            Debug.Log("has collisions");
            if (colliders.Exists(x => x.transform.parent != null && x.transform.parent.TryGetComponent<Line>(out line)))
            {
                if (line.LineType == LineType.Ice)
                {
                    rb.drag = 0;
                    if (lastSpeed < 0 && rb.velocity.x > lastSpeed) rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
                    if (lastSpeed > 0 && rb.velocity.x < lastSpeed) rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
                    /*if (rb.velocity.y > 0) 
                    {
                        Debug.Log("up");
                        rb.gravityScale = 0;
                    }*/
                    else rb.gravityScale = defaultGravity;
                }
            }
            else 
            {
                //rb.gravityScale = defaultGravity;
                rb.drag = defaultDrag;
            }
        } 
        else 
        {
            //rb.gravityScale = defaultGravity;
            rb.drag = defaultDrag;
        }
        lastSpeed = rb.velocity.x;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.transform.parent != null)
        {
            Line groundLine;
            if (col.gameObject.transform.parent.TryGetComponent<Line>(out groundLine) && groundLine.LineType == LineType.Rubber)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, BumpForce));
            }
        }
    }
}
