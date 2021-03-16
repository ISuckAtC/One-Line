using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Floats
    How to fight: Cannon using Grav Lines - Bounce back fireballs

    "Rythm": Fireballs - Dash - SlimeStorm while laughing Manically - Use cannon during SlimeStorm.*/

    public float maxMoveDist, moveSpeed, fireballSpeed, dashSpeed;
    public int slimeAmount;
    LayerMask pathBlockingElements;
    private bool Collided, pathBlocked, DestinationChange;
    [SerializeField]
    private int executions, fireballShots;
    private Vector3 Destination;
    public Transform[] FireballAttackPos, SlimeSpawnPos;
    public GameObject Fireball, SlimePrefab;
    public Transform PlayerTransfom;
    private Rigidbody2D RB2D;
    private Vector2 DashDir;


    void Start()
    {
        
        pathBlockingElements = 1 << LayerMask.NameToLayer("Line");
        fireballShots = 0;
        RB2D = gameObject.GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.L)) WizardMove();
        if(Input.GetKeyDown(KeyCode.K)) ShootFireball();
        if(Input.GetKeyDown(KeyCode.I)) DashAttack(false);
        if(Input.GetKeyDown(KeyCode.J)) StartCoroutine(FireballAttack(1, false));
        if(Input.GetKeyDown(KeyCode.P)) StartCoroutine(FireballAttack(1, true));

    }

    void ShootFireball()
    {

        GameObject fireball = Instantiate(Fireball, transform.position, transform.rotation);
        fireball.transform.up = PlayerTransfom.position - transform.position;
        fireball.GetComponent<Rigidbody2D>().velocity = fireball.transform.up * fireballSpeed;

    }

    IEnumerator FireballAttack(float AttackDelay, bool InSequence)
    {

        if(fireballShots <= FireballAttackPos.Length - 1)
        {

            transform.position = FireballAttackPos[fireballShots].position;
            Destination = FireballAttackPos[fireballShots].position;
            ShootFireball();
            fireballShots++;
            yield return new WaitForSeconds(AttackDelay);
            StartCoroutine(FireballAttack(AttackDelay, InSequence));

        }
        else if(fireballShots > FireballAttackPos.Length - 1)
        {

            int RandomNum = Random.Range(0, FireballAttackPos.Length - 1);
            transform.position = FireballAttackPos[RandomNum].position;
            Destination = FireballAttackPos[RandomNum].position;
            fireballShots = 0;

            if(InSequence)
            {

                yield return new WaitForSeconds(AttackDelay);
                StartCoroutine(DashAttack(InSequence));

            }

        }

    }

    void SlimeStorm()
    {

        foreach (Transform T in SlimeSpawnPos)
        {
            
            for(int i = 0; i < slimeAmount; i++)
            {

                GameObject spawnedSlime = Instantiate(SlimePrefab, new Vector2(T.position.x + (i - (slimeAmount / 2)), T.position.y), Quaternion.identity);

            }

        }

    }

    IEnumerator DashAttack(bool InSequence)
    {

        DashDir = new Vector2(PlayerTransfom.position.x - transform.position.x, PlayerTransfom.position.y - transform.position.y).normalized;

        if(Collided)
        {

            Destination = FireballAttackPos[FireballAttackPos.Length - 1].position;
            StartCoroutine(Move(1));
            Collided = false;

        }
        else
        {

            RB2D.velocity = DashDir * dashSpeed;
            yield return new WaitForFixedUpdate();
            StartCoroutine(DashAttack(InSequence));

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Line")
        {



        }

        if(collision.gameObject.tag == "Player")
        {

            collision.gameObject.GetComponent<PlayerController>().Kill(collision.transform.position, true);

        }

        Collided = true;

    }

    IEnumerator Move(float firstMoveWait)
    {

        RB2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(firstMoveWait);

        if(transform.position != Destination)
        {

            transform.position = Vector2.MoveTowards(transform.position, Destination, moveSpeed);
            yield return new WaitForFixedUpdate();
            StartCoroutine(Move(0));

        }

    }

    void WizardMove()
    {

        Vector2 moveToNewPos;
        RaycastHit2D hit2D;

        moveToNewPos = new Vector2(transform.position.x + Random.Range(-maxMoveDist, maxMoveDist), transform.position.y + Random.Range(-maxMoveDist, maxMoveDist));

        if(hit2D = Physics2D.Linecast(transform.position, moveToNewPos, pathBlockingElements)) pathBlocked = true;
        else pathBlocked = false;

        Debug.DrawRay(transform.position, moveToNewPos, Color.green, 5);

        if(!pathBlocked)
        {

            executions = 0;
            Destination = moveToNewPos;            
            StartCoroutine(Move(0));

        }
        else if(pathBlocked && executions < 50)
        {

            executions++;
            WizardMove();

        }
        else
        {

            executions = 0;

        }

    }

}
