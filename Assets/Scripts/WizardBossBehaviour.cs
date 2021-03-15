using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Floats
    How to fight: Cannon using Grav Lines - Bounce back fireballs

    "Rythm": Fireballs - Dash - SlimeStorm while laughing Manically - Use cannon during SlimeStorm.*/

    public float maxMove;
    LayerMask pathBlockingElements;
    private bool pathBlocked;
    private int executions;


    void Start()
    {
        


    }

    void Update()
    {
        


    }

    void WizardMove()
    {

        Vector2 moveToNewPos;
        RaycastHit2D hit2D;

        moveToNewPos = new Vector2(transform.position.x + Random.Range(-maxMove, maxMove), transform.position.y + Random.Range(-maxMove, maxMove));

        if(hit2D = Physics2D.Linecast(transform.position, moveToNewPos, pathBlockingElements)) pathBlocked = true;

        if(!pathBlocked)
        {

            

        }
        else if(pathBlocked && executions < 50) WizardMove();

    }

}
