using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct BehaviorPattern
{
    public Vector3 Position;
    public float WaitStep;
}
public enum Behavior
{
    Constant,
    ShootMove
}
public class Imp : AimTurret
{
    public Behavior behavior;
    public BehaviorPattern[] PatrolPattern;
    public float Speed;
    private int patrol;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        patrol = 0;
        switch (behavior)
        {
            case Behavior.Constant:
                StartCoroutine(ConstantBehavior(PatrolPattern));
                break;
            case Behavior.ShootMove:
                CancelInvoke(nameof(Fire));
                StartCoroutine(ShootMoveBehavior(PatrolPattern));
                break;
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    IEnumerator ConstantBehavior(BehaviorPattern[] pattern)
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pattern[patrol].Position, Speed);
            if (transform.position == pattern[patrol].Position) 
            {
                yield return new WaitForSeconds(pattern[patrol].WaitStep);
                patrol = patrol + 1 < PatrolPattern.Length ? patrol + 1 : 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator ShootMoveBehavior(BehaviorPattern[] pattern)
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pattern[patrol].Position, Speed);
            if (transform.position == pattern[patrol].Position) 
            {
                yield return new WaitForSeconds(pattern[patrol].WaitStep);
                Fire();
                patrol = patrol + 1 < PatrolPattern.Length ? patrol + 1 : 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public override void Death()
    {
        // DEATH LOGIC HERE
    }
}
