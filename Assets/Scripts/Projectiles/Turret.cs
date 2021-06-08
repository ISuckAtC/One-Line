using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy, IActivatable
{
    public GameObject bullet;
    public float fireRate = 1;
    public float waitBeforeStart = 8;
    public float bulletSpeed = 6;
    public bool Inactive;
    new public void Start()
    {
        base.Start();
        InvokeRepeating(nameof(TurretShoot), waitBeforeStart, fireRate);
    }
    private void TurretShoot()
    {
        if (Inactive) return;
        GetComponent<TurretAudioController>().playShootClip();
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        newBullet.transform.rotation = transform.rotation;
        GetComponent<Animator>().Play("Turret_fire");
    }

    public override void Death()
    {
        TurretAudioController _tac = GetComponent<TurretAudioController>();
        _tac.playDeathClip();
        CancelInvoke(nameof(TurretShoot));
        GetComponent<Animator>().speed = 0f;
        Destroy(gameObject, _tac.turretDeathClip.length);
    }

    public void Activate()
    {
        Inactive = false;
    }
}
