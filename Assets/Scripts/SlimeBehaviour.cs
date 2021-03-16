using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{

    private GameObject Player;
    private LayerMask EnemyMask;
    private Vector2 SlimeMoveDir, TempVector2;
    private float AggroTimer, JumpTimer, LastAttackTimer;
    public float AggroTime, JumpTime, ForwardJump, Jumpheight, AttackSpeed, CrushVelocity;
    [SerializeField]
    private bool HasSeenPlayer;
    private Rigidbody2D RB2D;
    private PlayerController PlayerControl;
    public GameObject SlimeDeath;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.Find("Player 1");
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        EnemyMask = ~((1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Air")));
        PlayerControl = Player.GetComponent<PlayerController>();
        JumpTimer = JumpTime;

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit2D;

        hit2D = Physics2D.Linecast(transform.position, Player.transform.position, EnemyMask);

        Debug.DrawLine(transform.position, Player.transform.position, Color.green, 5f);

        if (hit2D.collider.name == ("Player 1"))
        {

            HasSeenPlayer = true;

            TempVector2 = Player.transform.position - transform.position;

            if (TempVector2.normalized.x < 0)
                SlimeMoveDir = -Vector2.right;
            else if (TempVector2.normalized.x > 0)
                SlimeMoveDir = Vector2.right;
            else
                SlimeMoveDir = Vector2.zero;

        }
        else
            HasSeenPlayer = false;

        if(AggroTimer > 0)
        {

            if (JumpTimer < 0)
            {

                RB2D.velocity = new Vector2(SlimeMoveDir.x * ForwardJump, Jumpheight);
                JumpTimer = JumpTime;

            }

        }

        if (HasSeenPlayer)
            AggroTimer = AggroTime;
        else
            AggroTimer -= Time.deltaTime;

        if (AggroTimer > 0)
        {

            JumpTimer -= Time.deltaTime;

        }

        LastAttackTimer -= Time.deltaTime;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.name == "Player 1")
        {

            PlayerControl.Kill(gore:true);

        }

        Rigidbody2D OtherRB2D;
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out OtherRB2D))
        {

            Debug.Log(collision.gameObject.name);

            if(collision.gameObject.tag == "Line")
            {

                if(OtherRB2D.velocity.magnitude > CrushVelocity)
                {

                    Instantiate(SlimeDeath, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                } else
                {

                    Debug.Log(OtherRB2D.velocity.magnitude);

                }

            }

        }

        if (JumpTimer > 0 && JumpTimer < JumpTime - 0.5f)
            RB2D.velocity = new Vector2(0, RB2D.velocity.y);

    }

}
