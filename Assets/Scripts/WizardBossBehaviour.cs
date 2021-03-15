using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Floats
    How to fight: Cannon using Grav Lines - Bounce back fireballs

    "Rythm": Fireballs - Dash - SlimeStorm while laughing Manically - Use cannon during SlimeStorm.*/

    public float maxMoveDist, moveSpeed;
    LayerMask pathBlockingElements;
    private bool pathBlocked, DestinationChange;
    private int executions;
    private Vector2 Destination, FireballAttackPos;


    void Start()
    {
        
        pathBlockingElements = 1 << LayerMask.NameToLayer("Line");

    }

    void Update()
    {

        if(Input.GetKey(KeyCode.L)) WizardMove();

    }

    void FixedUpdate()
    {

        transform.position = Vector2.MoveTowards(transform.position, Destination, moveSpeed);

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
