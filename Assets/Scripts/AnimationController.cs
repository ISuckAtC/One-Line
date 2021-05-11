using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{

    public Animator playerAnim;
    public float MinSpeedForWalkAnim;

    void Update()
    {
        if ((GetComponent<Rigidbody2D>().velocity.x > MinSpeedForWalkAnim || -MinSpeedForWalkAnim > GetComponent<Rigidbody2D>().velocity.x) || (!GetComponent<PlayerMovement>().NukeMovement && Input.GetAxisRaw("Horizontal") != 0))
        {
            playerAnim.SetBool("isWalking", true);
            //Debug.Log("true");
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
            //Debug.Log("false");
        }
    }
}