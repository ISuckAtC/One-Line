using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSoundController : MonoBehaviour
{
    public AudioSource _bulletSource;

    public AudioClip normalCollisionClip,rubberCollisionClip, iceCollisionClip, gravityCollisionClip, destroyedClip;

    private void Start()
    {
        normalCollisionClip = Resources.Load<AudioClip>("Sound/TurretSounds/BulletImpact");
        rubberCollisionClip = Resources.Load<AudioClip>("Sound/TurretSounds/BulletRubberImpact");
        iceCollisionClip = Resources.Load<AudioClip>("Sound/TurretSounds/BulletIceImpact");
        gravityCollisionClip = Resources.Load<AudioClip>("Sound/TurretSounds/BulletImpact");
        destroyedClip = Resources.Load<AudioClip>("Sound/TurretSounds/BulletDeath");
    }


    public void playNormalCollisionClip()
    {
        _bulletSource.pitch = 1f;
        _bulletSource.PlayOneShot(normalCollisionClip);
    }
    public void playIceCollisionClip()
    {
        _bulletSource.pitch = 1f;
        _bulletSource.PlayOneShot(iceCollisionClip);
    }
    public void playRubberCollisionClip()
    {
        _bulletSource.pitch = .5f;
        _bulletSource.PlayOneShot(rubberCollisionClip);
    }
    public void playGravityCollisionClip()
    {
        _bulletSource.pitch = 1f;
        _bulletSource.PlayOneShot(gravityCollisionClip);
    }

    public void playDestroyClip()
    {
        _bulletSource.pitch = 1f;
        _bulletSource.PlayOneShot(destroyedClip);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Collision with ground
            //Destroyed or just collision?
            //playDestroyClip();
        }
        print("Collision  " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            //Collision with a line
            if (other.transform.parent.TryGetComponent(out Line line))
            {
                if (line.LineType == LineType.Ice)
                {
                    //Collision with ice line
                    playIceCollisionClip();
                }

                if (line.LineType == LineType.Normal)
                {
                    //Collision with normal line
                    playNormalCollisionClip();
                }

                if (line.LineType == LineType.Rubber)
                {
                    //Collision with rubber line
                    playRubberCollisionClip();
                }

                if (line.LineType == LineType.Weight)
                {
                    //Collision with gravity line
                    playGravityCollisionClip();
                }
            }
        }
    }
}
