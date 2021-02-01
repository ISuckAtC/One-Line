﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject BloodPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(Instantiate(BloodPrefab, col.GetContact(0).point, Quaternion.identity), 60);
        Destroy(col.collider.gameObject);
    }
}
