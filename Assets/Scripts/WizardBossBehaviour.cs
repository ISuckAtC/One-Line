using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Floats
    How to fight: Cannon using Grav Lines - Bounce back fireballs

    "Rythm": Fireballs - Dash - SlimeStorm while laughing Manically - Use cannon during SlimeStorm.*/

    public float maxMoveDist, moveSpeed, fireballSpeed;
    LayerMask pathBlockingElements;
    private bool pathBlocked, DestinationChange;
    [SerializeField]
    private int executions, i;
    private Vector2 Destination;
    public Transform[] FireballAttackPos;
    public GameObject Fireball;
    public Transform PlayerTransfom;


    void Start()
    {
        
        pathBlockingElements = 1 << LayerMask.NameToLayer("Line");
        i = 0;

    }

    void Update()
    {

        if(Input.GetKey(KeyCode.L)) WizardMove();
        if(Input.GetKey(KeyCode.K)) ShootFireball();
        if(Input.GetKeyDown(KeyCode.J)) StartCoroutine(FireballAttack(1.5f));

    }

    void FixedUpdate()
    {

        transform.position = Vector2.MoveTowards(transform.position, Destination, moveSpeed);

    }

    void ShootFireball()
    {

        GameObject fireball = Instantiate(Fireball, transform.position, transform.rotation);
        fireball.transform.up = PlayerTransfom.position - transform.position;
        fireball.GetComponent<Rigidbody2D>().velocity = fireball.transform.up * fireballSpeed;

    }

    IEnumerator FireballAttack(float AttackDelay)
    {

        if(i <= FireballAttackPos.Length - 1)
        {

            transform.position = FireballAttackPos[i].position;
            Destination = FireballAttackPos[i].position;
            ShootFireball();
            i++;
            yield return new WaitForSeconds(AttackDelay);
            StartCoroutine(FireballAttack(AttackDelay));

        }
        else if(i > FireballAttackPos.Length - 1)
        {

            int RandomNum = Random.Range(0, FireballAttackPos.Length - 1);
            transform.position = FireballAttackPos[RandomNum].position;
            Destination = FireballAttackPos[RandomNum].position;
            i = 0;

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
