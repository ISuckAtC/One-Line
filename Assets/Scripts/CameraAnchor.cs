using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour, IActivatable
{
    public float CamSize;
    public float PanSpeed;
    private float startCamSize;
    private bool active;
    private bool pan;

    void Start()
    {
        startCamSize = Camera.main.orthographicSize;
    }

    public void Update()
    {
        if (pan && Vector2.Distance(Camera.main.transform.localPosition, Vector2.zero) > 0)
        {
            Vector3 newPos = Vector2.MoveTowards(Camera.main.transform.localPosition, Vector2.zero, PanSpeed * Time.deltaTime);
            newPos.z = -10;
            Camera.main.transform.localPosition = newPos;
            if (Vector2.Distance(Camera.main.transform.localPosition, Vector2.zero) == 0 && !active) pan = false;
        }
    }

    public void Activate()
    {
        pan = true;
        if (active)
        {
            Camera.main.transform.parent = GameControl.main.Player.transform;
            Camera.main.orthographicSize = startCamSize;
        }
        else
        {
            Camera.main.transform.parent = transform;
            Camera.main.orthographicSize = CamSize;
        }
        active = !active;
    }
}
