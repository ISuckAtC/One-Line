using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slime : Enemy
{
    private GameObject target;
    public int[] TargetMask;
    private int targetMask;
    public float Range;
    public float MinForce, MaxForce;
    public float JumpCD;
    private bool jumping;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        targetMask = 0;
        foreach(int i in TargetMask) targetMask = targetMask | (1 << i);
    }

    void FixedUpdate()
    {
        List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, Range).ToList();
        if (!hits.Exists(x => x.gameObject == target)) target = null;
        if (!target)
        {
            int h = hits.Count;
            hits = hits.Where(x => x.gameObject.layer != 0 && ((1 << x.gameObject.layer) & targetMask) == 1 << x.gameObject.layer).ToList();

            if (hits.Count > 0)
            {
                hits.OrderBy(x => Vector2.Distance(x.transform.position, transform.position));
                target = hits[hits.Count - 1].gameObject;
                Debug.Log(gameObject.name + " aquired target: " + target.name);
            }
        }
    }

    public void Jump()
    {
        if (!target) return;
        Debug.Log("Jumping");
        Vector2 dir = new Vector2(target.transform.position.x > transform.position.x ? 1 : -1, 0);
        dir.y += Random.Range(0.3f, 4);
        rb.velocity = dir.normalized * Random.Range(MinForce, MaxForce);

        jumping = false;
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground") && !jumping)
        {
            jumping = true;
            Invoke(nameof(Jump), JumpCD);
        }
    }
    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumping = false;
            CancelInvoke(nameof(Jump));
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}
