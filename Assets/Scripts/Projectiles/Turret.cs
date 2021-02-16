using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate = 1;
    public float waitBeforeStart = 8;

    void Start()
    {

        InvokeRepeating(nameof(TurretShoot), waitBeforeStart, fireRate);
    }
    private void TurretShoot()
    {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.up*1.02f, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * 3;
        newBullet.transform.rotation = transform.rotation;
    }
}
