using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Movement
    [Range(0, 10)]
    public float moveSpeed;
    [Tooltip("should be between 0 and 1")]
    public float controlPowerInAir;
    public bool sprintOnOff;
    [Tooltip("should be the Player physicsMaterial 2D")]
    public PhysicsMaterial2D PM2D;
    private float yVel, jumpPower;
    private Vector2 movementVector;
    //Jumping
    public float BumpForce, jumpForce;
    private float playerControlPower, speedMultiplier, xMoveDir;
    private int jumpOnOff;
    //GroundCheck
    private float yGroundCheckOffset, groundCheckDist;
    private bool isGrounded;
    private LayerMask maskPlayer;
    //References
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb2D;
    private GameControl gc;

    void Start()
    {

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
        gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        maskPlayer = ~(((1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Air"))));
        rb2D.sharedMaterial = PM2D;
        capsuleCollider.sharedMaterial = PM2D;
        speedMultiplier = 1f;
        yGroundCheckOffset = -0.4f * transform.localScale.y;
        groundCheckDist = 0.5f * transform.localScale.y;
        jumpPower = 1f;

    }

    void FixedUpdate()
    {

        RaycastHit2D hit2D;

        if (hit2D = Physics2D.CircleCast(transform.position + new Vector3(0, yGroundCheckOffset, 0), 0.5f * transform.localScale.y, new Vector2(0, -1), groundCheckDist, maskPlayer))
        {
            if (hit2D.collider.isTrigger != true)
            {

                isGrounded = true;

                if (hit2D.collider.transform.parent != null)
                {
                    Line line;
                    if (hit2D.collider.transform.parent.TryGetComponent<Line>(out line))
                    {

                    }
                    else gc.ResetLineLimits();
                }
                else gc.ResetLineLimits();

                playerControlPower = 1;
                if (Input.GetKey(KeyCode.Space) && rb2D.velocity.y < jumpForce)
                {

                    jumpOnOff = 1;

                }

            }
            else isGrounded = false;

        }
        else isGrounded = false;

        if (isGrounded == false)
        {

            playerControlPower = controlPowerInAir;
            PM2D.friction = 0;
            rb2D.sharedMaterial = PM2D;
            capsuleCollider.sharedMaterial = PM2D;

        }
        else
        {

            playerControlPower = 1f;
            PM2D.friction = 0.6f;
            rb2D.sharedMaterial = PM2D;
            capsuleCollider.sharedMaterial = PM2D;

        }

        if (sprintOnOff)
        {

            if (Input.GetKey(KeyCode.LeftShift)) speedMultiplier = 1.6f;
            else speedMultiplier = 1f;

        }

        if (rb2D.velocity.x >= (moveSpeed * speedMultiplier) && Input.GetAxis("Horizontal") > 0) xMoveDir = 0;
        else
        if (rb2D.velocity.x <= (-moveSpeed * speedMultiplier) && Input.GetAxis("Horizontal") < 0) xMoveDir = 0;
        else xMoveDir = Input.GetAxis("Horizontal");

        yVel = rb2D.velocity.y - jumpForce;

        movementVector = new Vector2(rb2D.velocity.x + (speedMultiplier * xMoveDir * playerControlPower), rb2D.velocity.y + (-yVel * jumpOnOff * jumpPower));

        rb2D.velocity = movementVector;

        jumpOnOff = 0;

    }

}
