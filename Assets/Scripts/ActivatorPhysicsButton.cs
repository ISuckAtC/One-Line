using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorPhysicsButton : MonoBehaviour
{
    public GameObject[] Activatables;
    public float MaxDepress;
    public float ActuationPoint;
    public float SpringForce;
    public bool Toggle = true;
    public bool SingleUse;
    private bool active;
    private float heightUnDepressed;
    private Rigidbody2D rb;
    private bool completed;
    // Start is called before the first frame update
    void Start()
    {
        heightUnDepressed = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!completed)
        {
            if (!active && transform.position.y < heightUnDepressed - ActuationPoint)
            {
                active = true;
                foreach (GameObject activatable in Activatables) activatable.GetComponent<IActivatable>().Activate();
                if (SingleUse) completed = true;
            }
            if (active && transform.position.y > heightUnDepressed - ActuationPoint)
            {
                active = false;
                if (!Toggle) foreach (GameObject activatable in Activatables) activatable.GetComponent<IActivatable>().Activate();
            }
        }

        if (transform.position.y < heightUnDepressed - MaxDepress)
        {
            float a = transform.position.y;
            transform.position = new Vector3(transform.position.x, heightUnDepressed - MaxDepress, transform.position.z);
            //Debug.Log(a + " | " + transform.position.y);
        }
        if (transform.position.y > heightUnDepressed)
        {
            transform.position = new Vector3(transform.position.x, heightUnDepressed, transform.position.z);
        }
        else if (transform.position.y != heightUnDepressed) rb.AddForce(new Vector3(0f, SpringForce));
    }
}
