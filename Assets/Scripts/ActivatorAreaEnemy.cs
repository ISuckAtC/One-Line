using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivatorAreaEnemy : MonoBehaviour
{
    public GameObject CheckMark;
    public GameObject[] Enemies;
    public GameObject[] Activatables;
    bool once;
    private void FixedUpdate()
    {
        if (!Enemies.Any(x => x != null) && once == false) foreach (GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
            once = true;
                CheckMark.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
