using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public struct BehaviorPattern
{
    [SerializeField] private Transform TrackedPosition;
    public Vector3 Position {get {return TrackedPosition.position;}}
    public float WaitStep;
}
public class Imp : AimTurret
{
    public BehaviorPattern[] PatrolPattern;
    public float Speed;
    private int patrol;
    // Start is called before the first frame update
    new void Start()
    {
        patrol = 0;
        StartCoroutine(Patrol(PatrolPattern));
        base.Start();
    }

    new void FixedUpdate()
    {
        
        base.FixedUpdate();
    }

    IEnumerator Patrol(BehaviorPattern[] pattern)
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
}
