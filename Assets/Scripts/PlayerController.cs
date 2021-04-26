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
    private bool apex;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        defaultDrag = rb.drag;
        uiController = GameObject.FindObjectOfType<Canvas>().GetComponent<UiControl>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FallSpeed);
        }
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        if (capsule.GetContacts(colliders) > 0)
        {
            if (colliders.Exists(x => x.gameObject.layer == LayerMask.NameToLayer("Ground") || x.transform.parent != null && x.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line")))
            {
                apex = true;
                animator.ResetTrigger("Landing");
                animator.SetTrigger("Landing");
            }
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
            if (rb.velocity.y > 0)
            {
                apex = true;
                animator.ResetTrigger("MainCharacter_Jump");
                animator.SetTrigger("MainCharacter_Jump");
            }
            if (rb.velocity.y < 0 && apex)
            {
                apex = false;
                animator.ResetTrigger("MainCharacter_Jump_Down");
                animator.SetTrigger("MainCharacter_Jump_Down");
            }
            rb.drag = defaultDrag;
        }
        lastSpeed = rb.velocity;
    }
    public void Kill(Vector2? deathPosition = null, bool gore = true)
    {
        Camera.main.transform.parent = null;
        if (gore) Destroy(Instantiate(BloodPrefab, deathPosition != null ? (Vector3)deathPosition : transform.position, Quaternion.identity), 60);
        //GameControl.main.DeathCountText.text = (++GameControl.main.Global.ResetCount).ToString();
        Destroy(gameObject);
        uiController.PauseGameUiOnOff = true;
        UiControl.main.RestartText.SetActive(true);
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
