using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cannon : MonoBehaviour
{

    private GameObject closestEnemy, chamberedLine;
    public Transform BarrelTransform;
    public float CannonShotForce;
    public Transform targetTransform;
    public bool cannonLockOn, autoTarget, onStandby;

    // Start is called before the first frame update
    void Start()
    {

        if(targetTransform == null)
            autoTarget = true;

        onStandby = true;
        
    }

    void Update()
    {

        if(Input.GetKey(KeyCode.I))
            targetTransform = targetPicker();

    }

    void FixedUpdate()
    {
        if(cannonLockOn)
        {

            BarrelTransform.up = targetTransform.position - BarrelTransform.position;
            chamberedLine.transform.position = BarrelTransform.position;
            chamberedLine.transform.up = targetTransform.position - chamberedLine.transform.position;

        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        Line lineCheck;
        Debug.Log("Check0");

        if(onStandby)
        {

            if(col.gameObject.transform.parent.tag == "Line")
            {

                Debug.Log("Check1");
                if(col.gameObject.transform.parent.TryGetComponent<Line>(out lineCheck))
                {

                    Debug.Log("Check2");
                    if(lineCheck.LineType == LineType.Weight)
                    {

                        Debug.Log("Check3");
                        onStandby = false;
                        StartCoroutine(fireCannon(col.gameObject.transform.parent.gameObject));

                    }
                }                
            }
        }
    }

    IEnumerator fireCannon(GameObject lineToFire)
    {

        Rigidbody2D currentRB2D = lineToFire.GetComponent<Rigidbody2D>();
        chamberedLine = lineToFire;
        if(autoTarget)
            targetTransform = targetPicker();

        yield return new WaitForSeconds(1);

        currentRB2D.simulated = false;
        cannonLockOn = true;
        yield return new WaitForSeconds(2);

        currentRB2D.simulated = true;
        cannonLockOn = false;
        currentRB2D.velocity = lineToFire.transform.up * CannonShotForce;
        yield return new WaitForSeconds(1);

        onStandby = true;
        BarrelTransform.rotation = Quaternion.identity;

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
