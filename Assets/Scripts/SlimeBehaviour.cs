using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private LayerMask EnemyMask;
    private Vector2 SlimeMoveDir, TempVector2;
    [SerializeField]
    private float AggroTimer, JumpTimer, LastAttackTimer;
    public float AggroTime, JumpTime, ForwardJump, Jumpheight, AttackSpeed;
    [SerializeField]
    private bool HasSeenPlayer;
    [SerializeField]
    private Rigidbody2D RB2D;
    [SerializeField]
    private PlayerController PlayerControl;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.Find("Player 1");
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        EnemyMask = ~(1 << LayerMask.NameToLayer("Enemy"));
        PlayerControl = Player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit2D;

        hit2D = Physics2D.Linecast(transform.position, Player.transform.position, EnemyMask);

        Debug.DrawLine(transform.position, Player.transform.position, Color.green, 5f);

        if (hit2D.collider.name == ("Player 1"))
        {

            Debug.Log("Hit");

            HasSeenPlayer = true;

            TempVector2 = Player.transform.position - transform.position;

            if (TempVector2.normalized.x < 0)
                SlimeMoveDir = -Vector2.right;
            else if (TempVector2.normalized.x > 0)
                SlimeMoveDir = Vector2.right;
            else
                SlimeMoveDir = Vector2.zero;

            if (JumpTimer < 0)
            {

                RB2D.velocity = new Vector2(SlimeMoveDir.x * ForwardJump, Jumpheight);
                JumpTimer = JumpTime;

            }

        }
        else
            HasSeenPlayer = false;

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
        
        if(LastAttackTimer < 0)
        {

            if(collision.collider.name == "Player 1")
            {

                PlayerControl.Kill(true);
                LastAttackTimer = AttackSpeed;

            }

        }

    }

}
