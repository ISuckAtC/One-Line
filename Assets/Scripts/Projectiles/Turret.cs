﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    public GameObject bullet;
    public float fireRate = 1;
    public float waitBeforeStart = 8;
    public float bulletSpeed = 6;
    new public void Start()
    {
        base.Start();
        InvokeRepeating(nameof(TurretShoot), waitBeforeStart, fireRate);
    }
    private void TurretShoot()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        newBullet.transform.rotation = transform.rotation;
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}
