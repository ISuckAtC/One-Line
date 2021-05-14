using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBodyColCheck : MonoBehaviour
{

    private SlimeBehaviour slimeBehaviour;

    void Start()
    {

        slimeBehaviour = transform.parent.GetComponent<SlimeBehaviour>();

    }

    void OnCollisionStay2D(Collision2D col)
    {

        Rigidbody2D rb2D;

        if(col.gameObject.tag == "Player")
        {

            col.gameObject.GetComponent<PlayerController>().Kill(null, true);

        }
        else if(col.gameObject.TryGetComponent<Rigidbody2D>(out rb2D))
            if(col.gameObject.tag == "Line")
                if(rb2D.velocity.magnitude > slimeBehaviour.CrushVelocity)
                    slimeBehaviour.SlimeDie();

    }

}
