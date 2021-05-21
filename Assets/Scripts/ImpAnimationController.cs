using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpAnimationController : MonoBehaviour
{
    public Animator impAnim;
    
    public void Idle()
    {
        Debug.Log("Imp Idling");
        impAnim.SetBool("IsAttacking", false);
        impAnim.SetBool("IsDashing", false);
        impAnim.SetBool("IsDying", false);
        impAnim.SetBool("IsIdle", true);

    }

    public void Attacking()
    {
        Debug.Log("Imp Attacking");
        impAnim.SetBool("IsAttacking", true);
        impAnim.SetBool("IsDashing", false);
        impAnim.SetBool("IsDying", false);
        impAnim.SetBool("IsIdle", false);

    }

    public void Dashing()
    {
        Debug.Log("Imp Dashing");
        impAnim.SetBool("IsAttacking", false);
        impAnim.SetBool("IsDashing", true);
        impAnim.SetBool("IsDying", false);
        impAnim.SetBool("IsIdle", false);

    }

    public void Dying()
    {

        impAnim.SetBool("IsAttacking", false);
        impAnim.SetBool("IsDashing", false);
        impAnim.SetBool("IsDying", true);
        impAnim.SetBool("IsIdle", false);

    }
}