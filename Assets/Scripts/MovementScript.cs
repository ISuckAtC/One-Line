using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    [Range(0, 1)]
    public float moveSpeed;
    public float BumpForce;
    public float jumpForce;
    [SerializeField]
    private float playerControlPower;
    private float speedMultiplier = 1f;
    private float yGroundCheckOffset;
    private float xMoveDir;
    private int jumpOnOff;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2D;
    [SerializeField]
    private Vector2 movementVector;
    private LayerMask maskPlayer;

    // Start is called before the first frame update
    void Start()
    {

        maskPlayer = ~(1 << LayerMask.NameToLayer("Player"));
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
        yGroundCheckOffset = -0.05f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit2D hit2D;

        if (Input.GetKey(KeyCode.LeftControl))
        {

            capsuleCollider.size = new Vector2(1f, 1f);
            spriteRenderer.size = new Vector2(1f, 1f);
            yGroundCheckOffset = 0.45f;

        }
        else
        {

            capsuleCollider.size = new Vector2(1f, 2f);
            spriteRenderer.size = new Vector2(1f, 2f);
            yGroundCheckOffset = -0.05f;

        }

        if (hit2D = Physics2D.CircleCast(transform.position + new Vector3(0, yGroundCheckOffset, 0), 0.5f, Vector2.down, 1f, maskPlayer))
        {

            playerControlPower = 1;
            if (Input.GetKey(KeyCode.Space))
            {

                jumpOnOff = 1;

            }

        }
        else
        {

            jumpOnOff = 0;
            playerControlPower = 0.6f;

        }

        if (Input.GetKey(KeyCode.LeftShift)) speedMultiplier = 1.6f;
        else speedMultiplier = 1f;

        if (rb2D.velocity.x >= (5 * speedMultiplier) && Input.GetAxisRaw("Horizontal") == 1) xMoveDir = 0;
        else
        if (rb2D.velocity.x <= (-5 * speedMultiplier) && Input.GetAxisRaw("Horizontal") == -1) xMoveDir = 0;
        else xMoveDir = Input.GetAxisRaw("Horizontal");

        movementVector = new Vector2(rb2D.velocity.x + (moveSpeed * xMoveDir * playerControlPower), rb2D.velocity.y + (jumpForce * jumpOnOff));

        rb2D.velocity = movementVector;

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.GetComponent<Line>().LineType == LineType.Rubber)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, BumpForce));
        }
        
    }

}
