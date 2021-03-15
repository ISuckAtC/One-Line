using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float BumpForce, FallSpeed, SlideVelocityThreshold, IceSpeedMod;
    public GameObject BloodPrefab;
    private Rigidbody2D rb;
    private float defaultGravity, defaultDrag;
    private Vector2 lastSpeed;
    private UiControl uiController;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        defaultDrag = rb.drag;
        uiController = GameObject.FindObjectOfType<Canvas>().GetComponent<UiControl>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FallSpeed);
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        if (capsule.GetContacts(colliders) > 0)
        {
            Line line = null;
            if (colliders.Exists(x => x.transform.parent != null && x.transform.parent.TryGetComponent<Line>(out line)))
            {
                if (line.LineType == LineType.Ice)
                {
                    rb.drag = 0;
                    if (rb.velocity.magnitude != 0 && rb.velocity.y > 0)
                    {
                        if (lastSpeed.x < -SlideVelocityThreshold && rb.velocity.magnitude < lastSpeed.magnitude)
                        {
                            rb.velocity = (rb.velocity / rb.velocity.magnitude) * lastSpeed.magnitude * IceSpeedMod;
                            rb.velocity = new Vector2(rb.velocity.x * (rb.velocity.x < 0 ? 1 : -1), rb.velocity.y);
                        }
                        if (lastSpeed.x > SlideVelocityThreshold && rb.velocity.magnitude < lastSpeed.magnitude)
                        {
                            rb.velocity = (rb.velocity / rb.velocity.magnitude) * lastSpeed.magnitude * IceSpeedMod;
                            rb.velocity = new Vector2(rb.velocity.x * (rb.velocity.x < 0 ? -1 : 1), rb.velocity.y);
                        }
                    }
                }
            }
            else
            {
                rb.drag = defaultDrag;
            }
        }
        else
        {
            rb.drag = defaultDrag;
        }
        lastSpeed = rb.velocity;
    }
    public void Kill(Vector2? deathPosition = null, bool gore = true)
    {
        Camera.main.transform.parent = null;
        if (gore) Destroy(Instantiate(BloodPrefab, deathPosition != null ? (Vector3)deathPosition : transform.position, Quaternion.identity), 60);
        Destroy(gameObject);
        uiController.PauseGameUiOnOff = true;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.transform.parent != null)
        {
            Line groundLine;
            if (col.gameObject.transform.parent.TryGetComponent<Line>(out groundLine))
            {
                groundLine.Refund = false;
                if (groundLine.LineType == LineType.Rubber) GetComponent<Rigidbody2D>().AddForce(new Vector2(0, BumpForce));
            }
        }
    }
}
