using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivatorAreaEnemy : MonoBehaviour
{
    public GameObject[] CheckMarks;
    public GameObject[] Enemies;
    public GameObject[] Activatables;
    bool once;
    int activations;
    private void FixedUpdate()
    {
        if (!once && Enemies.Any(x => x == null))
        {
            if (CheckMarks.Length >= activations) CheckMarks[activations].GetComponent<SpriteRenderer>().color = Color.green;
            activations++;
            Enemies = Enemies.Where(x => x != null).ToArray();
            if (Enemies.Length == 0) 
            {
                foreach(GameObject activatable in Activatables) activatable.GetComponent<SpriteRenderer>().color = Color.green;
                once = true;
            }
        }
        /*if (!Enemies.Any(x => x != null) && once == false) foreach (GameObject activatable in Activatables)
        {
            activatable.GetComponent<IActivatable>().Activate();
            once = true;
                CheckMark.GetComponent<SpriteRenderer>().color = Color.green;
        }*/
    }
}
