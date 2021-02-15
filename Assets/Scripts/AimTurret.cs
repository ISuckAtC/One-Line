﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimTurret : MonoBehaviour
{
    public float Range;
    public float BulletSpawnOffset;
    public float BulletSpeed;
    public float FireRate;
    public GameObject BulletPrefab;
    private GameObject target;
    public int[] TargetMask;
    private int targetMask;

    // Start is called before the first frame update
    void Start()
    {
        targetMask = 0;
        foreach(int i in TargetMask) targetMask = targetMask | i;
        InvokeRepeating(nameof(Fire), 0, FireRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        List<Collider2D> hits = Physics2D.OverlapCircleAll(transform.position, Range).ToList();
        if (!hits.Exists(x => x.gameObject == target)) target = null;
        if (!target)
        {
            int h = hits.Count;
            hits = hits.Where(x => x.gameObject.layer != 0 && (x.gameObject.layer & targetMask) == x.gameObject.layer).ToList();
            Debug.Log(h + " | " + hits.Count);
            if (hits.Count > 0)
            {
                hits.OrderBy(x => Vector2.Distance(x.transform.position, transform.position));
                target = hits[hits.Count - 1].gameObject;
            }
        }
        if (target)
        {
            transform.up = target.transform.position - transform.position;
        }
    }

    void Fire()
    {
        if (target)
        {
            GameObject bullet = Instantiate(BulletPrefab, transform.position + (transform.up * BulletSpawnOffset), transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.up * BulletSpeed;
        }
    }
}
