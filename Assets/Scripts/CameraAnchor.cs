using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour, IActivatable
{
    [Tooltip("Smaller value = more view area")] public int CamSizePixelPerfect;
    public float PanSpeed;
    public bool Overview;
    private int startCamSize;
    private bool active;
    [HideInInspector] public bool Pan;

    void Start()
    {
        startCamSize = Camera.main.GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().assetsPPU;
    }

    public void Update()
    {
        if (Pan && Vector2.Distance(Camera.main.transform.localPosition, Vector2.zero) > 0)
        {
            Vector3 newPos = Vector2.MoveTowards(Camera.main.transform.localPosition, Vector2.zero, PanSpeed * Time.deltaTime);
            Camera.main.GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().assetsPPU = (int)Mathf.Lerp(
                CamSizePixelPerfect, GameControl.main.currentAnchor ? (GameControl.main.currentAnchor != this ? GameControl.main.currentAnchor.CamSizePixelPerfect : startCamSize) : startCamSize, 
                (1f / Vector2.Distance(transform.position, GameControl.main.Player.transform.position)) * Vector2.Distance(transform.position, (Vector2)Camera.main.transform.position));
            newPos.z = -10;
            Camera.main.transform.localPosition = newPos;
            if (Vector2.Distance(Camera.main.transform.localPosition, Vector2.zero) == 0 && !active) Pan = false;
        }
    }

    public void Activate()
    {
        Pan = true;
        if (active)
        {
            if (!Overview) GameControl.main.currentAnchor = null;
            Camera.main.transform.parent = GameControl.main.Player.transform;
        }
        else
        {
            if (!Overview) GameControl.main.currentAnchor = this;
            Camera.main.transform.parent = transform;
        }
        active = !active;
    }
}
