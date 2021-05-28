using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WizardBossBehaviour : MonoBehaviour
{

    /*LineWizard
    Abilites: Shoot fireballs - Dash - Spawn slimes - Fireball Rain
    How to fight: Cannon using Grav Lines - Bounce back fireballs*/

    public enum attackType
    {

        AllAttacks,
        FireballAttack,
        DashAttack,
        SlimeStorm,
        FireballRain,
        RandomAttack,
        Pause,
        StopAttacking,
        CompleteStop

    }

    public int Health;
    public GameObject[] Activatables, TravelPoints;
    public float maxMoveDist, moveSpeed, fireballSpeed, dashSpeed, AttackPause, FireballAttackSpeed, LineDestroyRadius, DeathDelay, DamageFlashTime;
    public int slimeAmount, fireballs;
    LayerMask pathBlockingElements;
    private bool Collided, pathBlocked, DestinationChange, slimeStage, bossActive, defeated;
    private int executions, fireballShots, attackStage, sequencePart, travelI;
    private Vector3 Destination;
    public Transform[] FireballAttackPos, SlimeSpawnPos;
    public GameObject Fireball, SlimePrefab, Cannon, NextStagePoint, ImpHead;
    public Transform PlayerTransfom, FireballRainPos;
    public SpriteRenderer[] WizardSpriteRenderers;
    private Rigidbody2D RB2D;
    private PolygonCollider2D Col2D;
    private Vector2 DashDir;
    [TextArea]
    public string HowToUseTheAttackPattern;
    private Quaternion originRotation;
    public attackType[] AttackPattern;
    private attackType attack;
    public List<GameObject> Slimes;
    private ImpAnimationController impAnimController;

    IEnumerator slimesCheck()
    {

        if(Slimes.Count > 0)
        {

            for(int i = 0; i < Slimes.Count - 1; i++)
            {

                if(Slimes[i] == null)
                {

                    Slimes.Remove(Slimes[i]);

                }

            }

            if(Slimes[0] == null)
            {

                Slimes.Remove(Slimes[0]);

            }

            if(Slimes.Count <= 0)
            {

                Death();

            }

        }

        yield return new WaitForSecondsRealtime(0.25f);
        StartCoroutine(slimesCheck());

    }

    void Start()
    {

        originRotation = transform.rotation;
        defeated = false;        
        Slimes = new List<GameObject>();
        pathBlockingElements = 1 << LayerMask.NameToLayer("Line") + LayerMask.NameToLayer("Ground");
        fireballShots = 0;
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        Col2D = gameObject.GetComponent<PolygonCollider2D>();
        if(Cannon == null && GameObject.FindGameObjectWithTag("Cannon") != null)
        {

            Cannon = GameObject.FindGameObjectWithTag("Cannon");
            Cannon.SetActive(false);

        }
        else if(Cannon != null)
            Cannon.SetActive(false);

        impAnimController = gameObject.GetComponentInChildren<ImpAnimationController>();

    }

    void Update()
    {

        if(PlayerTransfom == null)
            StopAllCoroutines();
        else
        if(PlayerTransfom.position.x < transform.position.x)
        {

            transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else if(PlayerTransfom.position.x > transform.position.x)
        {

            transform.rotation = Quaternion.Euler(180, 0, -90);

        }

        /*Vector3 tempVect = (transform.position - PlayerTransfom.position).normalized;

        ImpHead.transform.rotation = Quaternion.LookRotation(tempVect);*/

    }

    public void WizardBossActivate()
    {

        if(bossActive != true)
            Attack();

        bossActive = true;

        StartCoroutine(slimesCheck());

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
            int RandomNum = Random.Range(0, FireballAttackPos.Length);
            transform.position = FireballAttackPos[RandomNum].position;
            Destination = FireballAttackPos[RandomNum].position;
            StartCoroutine(SlimeStorm());
            break;

            case attackType.FireballRain:
            StartCoroutine(FireballRain());
            break;

            case attackType.RandomAttack:
            RandomAttack();
            break;

            case attackType.Pause:
            StartCoroutine(pauseFor(AttackPause * 2));
            break;

            case attackType.StopAttacking:
            stopAttacking();
            break;

        } 
    }

    void RandomAttack()
    {

        int RandomNum = Random.Range(0, 3);

        switch (RandomNum)
        {

            case 0:
            StartCoroutine(FireballAttack(FireballAttackSpeed, false));
            break;

            case 1:
            StartCoroutine(DashAttack(false));
            break;

            case 2:
            StartCoroutine(FireballRain());
            break;

        }

    }

    void stopAttacking()
    {
        GetComponent<ImpAudioController>().playWindUpBeforeAttack();
    }

    IEnumerator pauseFor(float pauseTime)
    {

        attackStage++;
        yield return new WaitForSeconds(pauseTime);

        Attack();

    }

    IEnumerator FireballRain()
    {

        impAnimController.Attacking();

        Destination = FireballRainPos.position;
        StartCoroutine(Move(0));
        yield return new WaitForSeconds(1.5f);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 140);
        float degPerIteration = 90/fireballs;

        for(int i = 0; i <= fireballs; i++)
        {

            GameObject fireball = Instantiate(Fireball, transform.position, transform.rotation);
            fireball.GetComponent<Rigidbody2D>().velocity = fireball.transform.up * fireballSpeed;
            transform.Rotate(Vector3.forward, degPerIteration);

        }

        yield return new WaitForSeconds(3);

        StartCoroutine(RestartSequence());

    }

    void ShootFireball()
    {
        GetComponent<ImpAudioController>().playFireballSound();
        GameObject fireball = Instantiate(Fireball, transform.position, transform.rotation);
        fireball.transform.up = PlayerTransfom.position - transform.position;
        fireball.GetComponent<Rigidbody2D>().velocity = fireball.transform.up * fireballSpeed;

    }

    IEnumerator FireballAttack(float AttackDelay, bool InSequence)
    {

        if(fireballShots <= FireballAttackPos.Length - 1)
        {

            Destination = FireballAttackPos[fireballShots].position;
            StartCoroutine(Move(0));
            yield return new WaitForSeconds(AttackDelay / 2);
            impAnimController.Attacking();
            yield return new WaitForSeconds(AttackDelay / 2);
            ShootFireball();
            fireballShots++;
            StartCoroutine(FireballAttack(AttackDelay, InSequence));
            impAnimController.Idle();

        }
        else if(fireballShots > FireballAttackPos.Length - 1)
        {

            impAnimController.Idle();

            fireballShots = 0;

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

        int RandomNum = Random.Range(0, FireballAttackPos.Length);
        transform.position = FireballAttackPos[RandomNum].position;
        Destination = FireballAttackPos[RandomNum].position;
        yield return new WaitForSeconds(AttackPause);

        Collided = false;
        StartCoroutine(DashAttackExecution(InSequence));
        GetComponent<ImpAudioController>().playDashSound();
        impAnimController.Dashing();

    }

    IEnumerator DashAttackExecution(bool InSequence)
    {

        yield return new WaitForEndOfFrame();

    }

    IEnumerator SlimeStorm()
    {
        GetComponent<ImpAudioController>().playSlimeSpawnSound();
        slimeStage = true;
        sequencePart++;

        foreach (Transform T in SlimeSpawnPos)
        {
            
            for(int i = 0; i < slimeAmount; i++)
            {

                GameObject spawnedSlime = Instantiate(SlimePrefab, new Vector2(T.position.x + (i - (slimeAmount / 2)), T.position.y), Quaternion.identity);
                
                Slimes.Add(spawnedSlime);

            }
        }

        if(Cannon != null)
        {

            Cannon.gameObject.SetActive(true);

        }

        yield return new WaitForSeconds(3);

        StartCoroutine(RestartSequence());

    }

    IEnumerator RestartSequence()
    {

        impAnimController.Idle();
        attackStage++;
        sequencePart = 0;
        slimeStage = false;
        yield return new WaitForSeconds(1);
        if(Cannon != null)
            Cannon.SetActive(false);
        Attack();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Projectile")
        {

            StartCoroutine(Hurt());

            if(slimeStage)
                StartCoroutine(RestartSequence());
            else
            {

                RB2D.velocity = Vector2.zero;
                StartCoroutine(Move(1));

            }

            Destroy(collision.gameObject);

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

        if(collision.transform.parent != null && collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Line") || collision.gameObject.layer == LayerMask.NameToLayer("Line") && collision.gameObject.GetComponent<Rigidbody2D>())
        {
            
            List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, LineDestroyRadius, 1 << LayerMask.NameToLayer("Line")).ToList();
            foreach (Collider2D hit in hits)
            {

                Destroy(hit.gameObject);
                
            }

        }

    }

    IEnumerator Hurt()
    {
        GetComponent<ImpAudioController>().playDamagesound();
        foreach(SpriteRenderer sR in WizardSpriteRenderers) sR.color = Color.red;

        Health--;

        if(Health <= 0)
            Death();
        else
        {

            yield return new WaitForSeconds(DamageFlashTime);

            foreach(SpriteRenderer sR in WizardSpriteRenderers) sR.color = Color.white;

        }

    }

    IEnumerator Move(float firstMoveWait)
    {

        RB2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(firstMoveWait);

        if(Vector2.Distance(transform.position, Destination) > 1)
        {

            transform.position = Vector2.MoveTowards(transform.position, Destination, moveSpeed);
            yield return new WaitForFixedUpdate();
            StartCoroutine(Move(0));

        }
        else if(defeated && TravelPoints.Length > travelI)
        {

            travelI += 1;
            GoNextStage();

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

    void GoNextStage()
    {

        if(travelI == TravelPoints.Length)
            Destroy(gameObject, DeathDelay);
        else
            Destination = TravelPoints[travelI].transform.position;
        StartCoroutine(Move(0));

    }

    void Death()
    {
        GetComponent<ImpAudioController>().playDeathSound();
        foreach (GameObject GO in Activatables)
        {
            
            GO.GetComponent<IActivatable>().Activate();

        }

        defeated = true;
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;

        StopAllCoroutines();

        impAnimController.Dying();

        if(TravelPoints.Length > 0)
        {

            travelI = 0;

            if(TravelPoints[0] != null)
                GoNextStage();

        }
        else
        {

            Destroy(gameObject, DeathDelay);

        }

    }

}