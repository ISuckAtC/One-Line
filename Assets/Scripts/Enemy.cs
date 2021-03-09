using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Health = 1;
    protected int health;
    public void Start()
    {
        health = Health;
    }
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) 
        {
            Death();
        }
    }
    public abstract void Death();
}
