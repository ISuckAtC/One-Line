using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioController : MonoBehaviour
{
    public AudioClip footStepsClip;
    public AudioClip jumpClip, fallingClip, landingClip;
    public AudioClip deathClip;
    public AudioClip whooshClip;
    
    private GameObject _player;
    private PlayerMovement _pm;

    private AudioSource playerAS, whooshSource;
    private Rigidbody2D playerRB;

    private bool fallOnce = false, hasBeenOnIce = false, playWhoosh = false;

    private float minWhooshPitch = 0.75f, maxWhooshPitch = 1.75f;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameControl.main.Player;
        _pm = _player.GetComponent<PlayerMovement>();
        playerRB = _player.GetComponent<Rigidbody2D>();
        
        footStepsClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_concrete_walk_01");
        jumpClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_effort_short_jump_03");
        fallingClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_death_03");
        landingClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_concrete_land_01");
        deathClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_death_15");
        whooshClip = Resources.Load<AudioClip>("Sound/PlayerSounds/wind_general_gusty_low_loop_03");

        if (_player.GetComponent<AudioSource>() == null)
        {
            playerAS = _player.AddComponent<AudioSource>();
            whooshSource = _player.AddComponent<AudioSource>();
        }
        else
        {
            playerAS = _player.GetComponent<AudioSource>();
        }
        
        
        InvokeRepeating("footSteps",0, footStepsClip.length * 1.2f);
        playerAS.volume = 0.05f;
        whooshSource.volume = 0.05f;
        
        startWhooshEffect();
    }

    private void FixedUpdate()
    {
        if (_pm.isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (playerRB.velocity.y > 0)
            {
                playJumpSound();
            }
        }

        if (_player != null)
        {
            if (_pm.shouldSlide)
            {
                //on ice
                playWhoosh = true;
            }
            else
            {
                whooshSource.pitch = 0f;
            }
            if (playWhoosh)
            {
                playWhooshEffect();
            }
        }
        
        /*if (_player != null)
        { //code for falling sound
            if (!_pm.isGrounded && playerRB.velocity.y < 0 && !fallOnce)
            {
                fallOnce = true;
                playFallingSound();
            }

            if (_pm.isGrounded)
            {
                fallOnce = false;
            }
        }*/
    }

    private void startWhooshEffect()
    {
        whooshSource.pitch = 0f;
        whooshSource.clip = whooshClip;
        whooshSource.loop = true;
        whooshSource.Play();
    }
    private void playWhooshEffect()
    {
       
        // if (playerRB.velocity.x > 1 || playerRB.velocity.x < -1)
        // {
        //     whooshSource.pitch = 1.1f;
        // }
        // else if (playerRB.velocity.x > 7 || playerRB.velocity.x < -7)
        // {
        //     whooshSource.pitch = 1.5f;
        // }

        float currSpeed = playerRB.velocity.magnitude;
        float pitchModifier = maxWhooshPitch - minWhooshPitch;
        whooshSource.pitch = minWhooshPitch + (currSpeed / 27f) * pitchModifier;
        Debug.Log("MAGNITUDE : " + playerRB.velocity.magnitude + " X:  " + playerRB.velocity.x);
        if (playerRB.velocity.x < 8.5f && playerRB.velocity.x > -8.5f)
        {
            whooshSource.pitch = 0f;
        }
    }
    
    private void footSteps()
    {
        if(_player == null)
            return;
        
        if(_player.GetComponent<Animator>().GetBool("isWalking") && _pm.isGrounded && playerRB.velocity.x > 1f || playerRB.velocity.x < -1f)
            playerAS.PlayOneShot(footStepsClip);
    }

    public void playJumpSound()
    {
        playerAS.PlayOneShot(jumpClip);
    }

    public void playFallingSound()
    {
        playerAS.PlayOneShot(fallingClip);
    }

    public void playLandingSound()
    {
        playerAS.PlayOneShot(landingClip);
    }

    public void playDeathSound()
    {
        playerAS.PlayOneShot(deathClip);
    }
}
