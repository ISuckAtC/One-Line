using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cannon : MonoBehaviour
{

    private GameObject closestEnemy;
    public Transform BarrelTransform;
    public float CannonShotForce;
    public Transform targetTransform;
    public bool cannonLockOn, autoTarget;

    // Start is called before the first frame update
    void Start()
    {

        if(targetTransform == null)
            autoTarget = true;
        
    }

    void Update()
    {

        if(Input.GetKey(KeyCode.I))
            targetTransform = targetPicker();

    }

    void FixedUpdate()
    {
        if(cannonLockOn)
            BarrelTransform.up = targetTransform.position - transform.position;
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        Line lineCheck;

        if(col.gameObject.tag == "Line")
            if(gameObject.TryGetComponent<Line>(out lineCheck))
                if(lineCheck.LineType == LineType.Weight)
                    StartCoroutine (fireCannon(col.gameObject));

    }

    IEnumerator fireCannon(GameObject lineToFire)
    {

        if(autoTarget)
            targetTransform = targetPicker();

        yield return new WaitForSeconds(1);

        Rigidbody2D currentRB2D = lineToFire.GetComponent<Rigidbody2D>();
        currentRB2D.simulated = false;
        cannonLockOn = true;
        yield return new WaitForSeconds(2);

        lineToFire.transform.up = targetTransform.position - lineToFire.transform.position;
        currentRB2D.velocity = Vector2.up * CannonShotForce;
        cannonLockOn = false;

    }

    Transform targetPicker()
    {

        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        float[] distances = new float[enemiesInScene.Length];

        for(int i = 0; i <= enemiesInScene.Length - 1; i++)
        {

            distances[i] = Vector2.Distance(transform.position, enemiesInScene[i].transform.position);

        }

        float shortestDistance = distances.Min();

        for(int i = 0; i <= distances.Length - 1; i++)
        {

            if(distances[i] == shortestDistance)
                closestEnemy = enemiesInScene[i];

        }

        return closestEnemy.transform;

    }

}
