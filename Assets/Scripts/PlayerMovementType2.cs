using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementType2 : MonoBehaviour
{

    public float gravity;

    private bool isGrounded;
    private int jumpOnOff;
    private float jumpForce, playerControlPower;
    private float yGroundCheckOffset, groundCheckDist;
    private Rigidbody2D rb2D;
    private GameControl gc;
    private LayerMask maskPlayer;
    [SerializeField]
    private Vector3 movementVector;

    void Start()
    {

        maskPlayer = ~(((1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Air"))));
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        yGroundCheckOffset = -0.4f * transform.localScale.y;
        groundCheckDist = 0.5f * transform.localScale.y;

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

        movementVector = new Vector3(Input.GetAxis("Horizontal") + rb2D.velocity.x, Input.GetAxis("Vertical") + rb2D.velocity.y - 0.2f, 0);

        if(!isGrounded)
            rb2D.AddForce(-Vector3.up * (gravity * rb2D.mass));

        rb2D.MovePosition(transform.position + movementVector);

    }

}
