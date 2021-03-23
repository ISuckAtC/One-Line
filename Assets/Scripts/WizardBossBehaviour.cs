using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Floats
    How to fight: Cannon using Grav Lines - Bounce back fireballs

    "Rythm": Fireballs - Dash - SlimeStorm while laughing Manically - Use cannon during SlimeStorm.*/

    public enum attackType
    {

        AllAttacks,
        FireballAttack,
        DashAttack,
        SlimeStorm,
        Pause

    }

    public float maxMoveDist, moveSpeed, fireballSpeed, dashSpeed, AttackPause, FireballAttackSpeed;
    public int slimeAmount;
    LayerMask pathBlockingElements;
    private bool Collided, pathBlocked, DestinationChange, slimeStage, bossActive;
    private int executions, fireballShots, attackStage, sequencePart;
    private Vector3 Destination;
    public Transform[] FireballAttackPos, SlimeSpawnPos;
    public GameObject Fireball, SlimePrefab, Cannon;
    public Transform PlayerTransfom;
    private Rigidbody2D RB2D;
    private CircleCollider2D Col2D;
    private Vector2 DashDir;
    [TextArea]
    public string HowToUseTheAttackPattern;
    public attackType[] AttackPattern;
    private attackType attack;

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
            WizardBossActivate();

    }


    void Start()
    {
        
        pathBlockingElements = 1 << LayerMask.NameToLayer("Line");
        fireballShots = 0;
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        Col2D = gameObject.GetComponent<CircleCollider2D>();
        Col2D.isTrigger = true;
        if(Cannon == null && GameObject.FindGameObjectWithTag("Cannon") != null)
        {

            Cannon = GameObject.FindGameObjectWithTag("Cannon");
            Cannon.SetActive(false);

        }
        else if(Cannon != null)
            Cannon.SetActive(false);

    }

    public void WizardBossActivate()
    {

        if(bossActive != true)
            Attack();

        bossActive = true;

    }

    private void Attack()
    {
        
        if(attackStage > AttackPattern.Length - 1)
            attackStage = 0;

        attack = AttackPattern[attackStage];

        switch (attack)
        {
            
            case attackType.AllAttacks:
            StartCoroutine(FireballAttack(FireballAttackSpeed, true));
            break;

            case attackType.FireballAttack:
            StartCoroutine(FireballAttack(FireballAttackSpeed, false));
            break;

            case attackType.DashAttack:
            StartCoroutine(DashAttack(false));
            break;

            case attackType.SlimeStorm:
            SlimeStorm();
            break;

            case attackType.Pause:
            StartCoroutine(pauseFor(AttackPause * 2));
            break;

        } 

    }

    IEnumerator pauseFor(float pauseTime)
    {

        attackStage++;
        yield return new WaitForSeconds(pauseTime);

        Attack();

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

            fireballShots = 0;
            Debug.Log("Fireball");

            if(InSequence)
            {

                sequencePart++;
                yield return new WaitForSeconds(AttackDelay);
                StartCoroutine(DashAttack(InSequence));

            }
            else
                StartCoroutine(RestartSequence());
            
        }
    }

    IEnumerator DashAttack(bool InSequence)
    {

        int RandomNum = Random.Range(0, FireballAttackPos.Length - 1);
        transform.position = FireballAttackPos[RandomNum].position;
        Destination = FireballAttackPos[RandomNum].position;
        yield return new WaitForSeconds(AttackPause);

        Collided = false;
        StartCoroutine(DashAttackExecution(InSequence));

    }

    IEnumerator DashAttackExecution(bool InSequence)
    {

        Debug.Log("Dash");

        Col2D.isTrigger = false;
        DashDir = new Vector2(PlayerTransfom.position.x - transform.position.x, PlayerTransfom.position.y - transform.position.y).normalized;

        if(Collided)
        {

            int RandomNum = Random.Range(0, FireballAttackPos.Length - 1);
            Destination = FireballAttackPos[RandomNum].position;
            StartCoroutine(Move(1));
            Collided = false;
            Col2D.isTrigger = true;

            if(InSequence)
            {

                sequencePart++;
                yield return new WaitForSeconds(2);
                SlimeStorm();

            }else
                StartCoroutine(RestartSequence());

        }
        else
        {

            RB2D.velocity = DashDir * dashSpeed;
            yield return new WaitForFixedUpdate();
            StartCoroutine(DashAttackExecution(InSequence));

        }
    }

    void SlimeStorm()
    {

        Debug.Log("Slime");
        slimeStage = true;
        Col2D.isTrigger = false;
        sequencePart++;

        foreach (Transform T in SlimeSpawnPos)
        {
            
            for(int i = 0; i < slimeAmount; i++)
            {

                GameObject spawnedSlime = Instantiate(SlimePrefab, new Vector2(T.position.x + (i - (slimeAmount / 2)), T.position.y), Quaternion.identity);

            }
        }

        if(Cannon != null)
        {

            Cannon.gameObject.SetActive(true);

        }

    }

    IEnumerator RestartSequence()
    {

        attackStage++;
        sequencePart = 0;
        slimeStage = false;
        yield return new WaitForSeconds(1);
        Cannon.SetActive(false);
        Col2D.isTrigger = true;
        Attack();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Projectile")
        {

            //Lose health here :3
            Debug.Log("BigOuch!");

            if(slimeStage)
                StartCoroutine(RestartSequence());
            else
            {

                RB2D.velocity = Vector2.zero;
                StartCoroutine(Move(1));

            }

        }
        else
        {

            RB2D.velocity = Vector2.zero;
            StartCoroutine(Move(1));

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