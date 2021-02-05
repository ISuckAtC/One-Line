using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TurretShoot), 1, 1);
    }
    private void TurretShoot()
    {
        GameObject newBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
