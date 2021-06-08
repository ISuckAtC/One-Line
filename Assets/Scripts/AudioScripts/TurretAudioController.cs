using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAudioController : MonoBehaviour
{
    public AudioSource TurretAS;
    public AudioClip turretShootClip, turretDeathClip;
    
    
    // Start is called before the first frame update
    void Start()
    {
        turretShootClip = Resources.Load<AudioClip>("Sound/TurretSounds/TurretShoot");
        turretDeathClip = Resources.Load<AudioClip>("Sound/TurretSounds/TurretDeath");
    }
    
    public void playShootClip()
    {
        TurretAS.PlayOneShot(turretShootClip);
    }

    public void playDeathClip()
    {
        TurretAS.PlayOneShot(turretDeathClip);
    }
}
