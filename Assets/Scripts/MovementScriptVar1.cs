using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScriptVar1 : MonoBehaviour
{

    [Range(0, 10)]
    public float moveSpeed = 0.6f;
    public float BumpForce;
    public float jumpForce;
    private bool isGrounded;
    private float yVel;
    private float crouchedMoveDebuf;
    [SerializeField]
    private float playerControlPower;
    private float speedMultiplier = 1f;
    private float yGroundCheckOffset;
    private float xMoveDir;
    private float jumpPower;
    private float groundCheckDist;
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
        groundCheckDist = 0.5f;
        crouchedMoveDebuf = 1f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit2D hit2D;

        if (Input.GetKey(KeyCode.LeftControl))
        {

            capsuleCollider.size = new Vector2(1f, 1f);
            spriteRenderer.size = new Vector2(1f, 1f);
            yGroundCheckOffset = -0.05f;
            groundCheckDist = 0f;
            jumpPower = 0.7f;
            crouchedMoveDebuf = 0.4f;

        }
        else
        {

            hit2D = Physics2D.CircleCast(transform.position + new Vector3(0, 0.4f, 0),
                0.5f, new Vector2(0, 1), 0.5f, maskPlayer);

            if (!hit2D)
            {

                capsuleCollider.size = new Vector2(1f, 1.98f);
                spriteRenderer.size = new Vector2(1f, 1f);
                yGroundCheckOffset = -0.05f;
                groundCheckDist = 0.5f;
                jumpPower = 1f;
                crouchedMoveDebuf = 1f;

            }

        }

        if (hit2D = Physics2D.CircleCast(transform.position + new Vector3(0, yGroundCheckOffset, 0), 0.5f, new Vector2(0, -1), groundCheckDist, maskPlayer))
        {

            isGrounded = true;

            playerControlPower = 1;
            if (Input.GetKey(KeyCode.Space) && rb2D.velocity.y < jumpForce)
            {

                jumpOnOff = 1;

            }

        }
        else isGrounded = false;

        if (isGrounded == false)
        {

            playerControlPower = 0.3f;

        }
        else playerControlPower = 1f;


        if (Input.GetKey(KeyCode.LeftShift)) speedMultiplier = 1.6f;
        else speedMultiplier = 0.8f;

        if (rb2D.velocity.x >= (moveSpeed * speedMultiplier * crouchedMoveDebuf) && Input.GetAxis("Horizontal") > 0) xMoveDir = 0;
        else
        if (rb2D.velocity.x <= (-moveSpeed * speedMultiplier * crouchedMoveDebuf) && Input.GetAxis("Horizontal") < 0) xMoveDir = 0;
        else xMoveDir = Input.GetAxis("Horizontal");

        yVel = rb2D.velocity.y - jumpForce;

        movementVector = new Vector2(rb2D.velocity.x + (speedMultiplier * xMoveDir * playerControlPower * crouchedMoveDebuf),rb2D.velocity.y + (-yVel * jumpOnOff * jumpPower));

        rb2D.velocity = movementVector;

        jumpOnOff = 0;

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.GetComponent<Line>().LineType == LineType.Rubber)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, BumpForce));
        }

    }

}
