using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public int fireRate = 1;

    void Start()
    {

        InvokeRepeating(nameof(TurretShoot), 5, fireRate);
    }
    private void TurretShoot()
    {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.up*1.019f, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * 3;
        newBullet.transform.rotation = transform.rotation;
    }
}
