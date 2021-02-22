using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate = 1;
    public float waitBeforeStart = 8;
    public float bulletSpeed = 6;
    public float startPointDist = 1.02f;
    void Start()
    {

        InvokeRepeating(nameof(TurretShoot), waitBeforeStart, fireRate);
    }
    private void TurretShoot()
    {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.up*startPointDist, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        newBullet.transform.rotation = transform.rotation;
    }
}
