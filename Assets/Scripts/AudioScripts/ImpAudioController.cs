using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpAudioController : MonoBehaviour
{
    public AudioSource _impAS;
    public AudioClip fireballClip, windUpClip, dashClip, slimeSpawnClip, damageClip, deathClip;
    
    // Start is called before the first frame update
    void Start()
    {
        #region FindFromResources
        fireballClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpFireball");
        windUpClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpWindUp");
        dashClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpDash");
        slimeSpawnClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpSlimeSpawn");
        damageClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpDamage");
        deathClip = Resources.Load<AudioClip>("Sound/ImpSounds/ImpDeath");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playFireballSound()
    {
        _impAS.PlayOneShot(fireballClip);
        print("SOUND  fireball");
    }

    public void playWindUpBeforeAttack()
    {
        _impAS.PlayOneShot(windUpClip);
        print("SOUND  windup");

    }

    public void playDashSound()
    {
        _impAS.PlayOneShot(dashClip);
        print("SOUND  dash");

    }

    public void playSlimeSpawnSound()
    {
       _impAS.PlayOneShot(slimeSpawnClip);
       print("SOUND  slimespawn");

    }

    public void playDamagesound()
    {
        _impAS.PlayOneShot(damageClip);
        print("SOUND  damage");

    }

    public void playDeathSound()
    {
        _impAS.PlayOneShot(deathClip);
        print("SOUND  death");
    }
    
}
