using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{

    private GameObject Player;
    private LayerMask EnemyMask;
    private Vector2 SlimeMoveDir, TempVector2;
    private float AggroTimer, JumpTimer, LastAttackTimer;
    public float AggroTime, JumpTime, ForwardJump, Jumpheight, AttackSpeed;
    private bool HasSeenPlayer;
    private Rigidbody2D RB2D;
    private PlayerController PlayerControl;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.Find("Player");
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        EnemyMask = ~(1 << LayerMask.NameToLayer("Player"));
        PlayerControl = Player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit2D;

        if (hit2D = Physics2D.Linecast(transform.position, Player.transform.position, EnemyMask))
        {

            if (hit2D.collider.name == ("Player"))
            {

                TempVector2 = Player.transform.position - transform.position;

                if (TempVector2.normalized.x < 0)
                    SlimeMoveDir = -Vector2.right;
                else if (TempVector2.normalized.x > 0)
                    SlimeMoveDir = Vector2.right;
                else
                    SlimeMoveDir = Vector2.zero;

                if(JumpTimer < 0)
                {

                    RB2D.velocity = new Vector2(SlimeMoveDir.x * ForwardJump, Jumpheight);
                    JumpTimer = JumpTime;

                }

            }

        } else
            HasSeenPlayer = false;

        if (HasSeenPlayer)
            AggroTimer = AggroTime;
        else
            AggroTimer -= Time.deltaTime;

        while (AggroTimer > 0)
        {

            JumpTimer -= Time.deltaTime;

        }

        LastAttackTimer -= Time.deltaTime;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if(LastAttackTimer < 0)
        {

            if(collision.collider.name == "Player")
            {

                PlayerControl.Kill(true);
                LastAttackTimer = AttackSpeed;

            }

        }

    }

}
