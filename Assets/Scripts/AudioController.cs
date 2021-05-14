using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioController : MonoBehaviour
{
    private AudioClip footStepsClip, normalFootstepClip, metalFootstepClip, iceFootstepClip, rubberFootstepClip;
    private AudioClip jumpClip, fallingClip, landingClip;
    private AudioClip deathClip;
    private AudioClip whooshClip;
    private AudioClip levelCompleteClip;
    private AudioClip hitHeadClip;
    
    private GameObject _player;
    private PlayerMovement _pm;

    public AudioSource playerAS, whooshSource;
    private Rigidbody2D playerRB;

    private bool fallOnce, hasBeenOnIce, playHeadHitOnce;

    private float minWhooshPitch = 0.75f, maxWhooshPitch = 1.75f;

    private LayerMask playerMask;

    private float landingTime, prevLandingTime, jumpingTime, prevJumpingTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameControl.main.Player;
        _pm = _player.GetComponent<PlayerMovement>();
        playerRB = _player.GetComponent<Rigidbody2D>();
        
        playerMask = ~((1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Air")) + (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Slimes")));

        #region FindFromResources
        normalFootstepClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_concrete_walk_01");
        footStepsClip = normalFootstepClip;
        metalFootstepClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_metal_high_run_05");
        iceFootstepClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_ice_crunchy_land_05_Edited");
        rubberFootstepClip = Resources.Load<AudioClip>("Sound/PlayerSounds/impact_deep_thud_bounce_01_Edited");
        
        jumpClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_effort_short_jump_03");
        fallingClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_death_03");
        landingClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_concrete_land_01");
        
        deathClip = Resources.Load<AudioClip>("Sound/PlayerSounds/voice_male_c_death_15");
        
        whooshClip = Resources.Load<AudioClip>("Sound/PlayerSounds/wind_general_gusty_low_loop_03");
        
        levelCompleteClip = Resources.Load<AudioClip>("Sound/PlayerSounds/footstep_concrete_run_17_Looping");
        
        hitHeadClip = Resources.Load<AudioClip>("Sound/PlayerSounds/AUD_HitHead");

        #endregion
        
        if (_player.GetComponent<AudioSource>() == null)
        {
            playerAS = _player.AddComponent<AudioSource>();
            whooshSource = _player.AddComponent<AudioSource>();
        }
        
        InvokeRepeating("footSteps",0, footStepsClip.length * 1.3f);

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
                playWhooshEffect();
            }
            else
            {
                whooshSource.pitch = 0f;
            }
        }

        RaycastHit2D rh2D;
        if (rh2D = Physics2D.Raycast(transform.position + new Vector3(0, 1,0), transform.up, 1f, playerMask))
        {
            if (rh2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && !playHeadHitOnce)
            {
                playHeadHitOnce = true;
                StartCoroutine("playHeadHit");
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
        float currSpeed = playerRB.velocity.magnitude;
        float pitchModifier = maxWhooshPitch - minWhooshPitch;
        whooshSource.pitch = minWhooshPitch + (currSpeed / 27f) * pitchModifier; //27 is just a random tested number
        if (playerRB.velocity.x < 8.5f && playerRB.velocity.x > -8.5f)
        {
            whooshSource.pitch = 0f;
        }
    }
    
    private void footSteps()
    {
        if(_player == null)
            return;
        
        //Code for different ground types
        RaycastHit2D rh2D;
        if (rh2D = Physics2D.Raycast(transform.position + new Vector3(0, -1f, 0), new Vector2(0, -1), 1f,
            playerMask))
        {
            Line thisLine;
            if (rh2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                footStepsClip = normalFootstepClip;
            }
            else if (rh2D.transform.gameObject.layer == LayerMask.NameToLayer("Line"))
            {
                if (rh2D.transform.parent == null)
                {
                    if (rh2D.transform.TryGetComponent<Line>(out thisLine))
                    {
                        if (thisLine.LineType == LineType.Weight)
                        {
                            footStepsClip = metalFootstepClip;
                        }
                    }
                }
                else //has parent
                {
                    if (rh2D.transform.parent.TryGetComponent<Line>(out thisLine))
                    {
                        if (thisLine.LineType == LineType.Ice)
                        {
                            footStepsClip = iceFootstepClip;
                        }

                        if (thisLine.LineType == LineType.Normal)
                        {
                            footStepsClip = normalFootstepClip;
                        }

                        if (thisLine.LineType == LineType.Rubber)
                        {
                            footStepsClip = rubberFootstepClip;
                        }
                    }
                    else //if it can't get the component Line
                    {
                        footStepsClip = normalFootstepClip;
                    }
                }
            }
        }
        
        if(_player.GetComponent<Animator>().GetBool("isWalking") && _pm.isGrounded && playerRB.velocity.x > 1f || playerRB.velocity.x < -1f)
            playerAS.PlayOneShot(footStepsClip);
    }

    public void playJumpSound()
    {
        jumpingTime = Time.time;
        if(jumpingTime < prevJumpingTime + .3f)
            return;
        playerAS.PlayOneShot(jumpClip);
        prevJumpingTime = Time.time;
    }

    public void playFallingSound()
    {
        playerAS.PlayOneShot(fallingClip);
    }

    public void playLandingSound()
    {
        landingTime = Time.time;
        if(landingTime < prevLandingTime + .3f)
            return;
        playerAS.PlayOneShot(landingClip);
        prevLandingTime = Time.time;
    }

    public void playDeathSound()
    {
        playerAS.PlayOneShot(deathClip);
    }

    public void playLevelCompleted()
    {
        playerAS.PlayOneShot(levelCompleteClip);
    }

    public IEnumerator playHeadHit()
    {
        playerAS.PlayOneShot(hitHeadClip);
        yield return new WaitForSeconds(hitHeadClip.length * 2f);
        playHeadHitOnce = false;
    }
}