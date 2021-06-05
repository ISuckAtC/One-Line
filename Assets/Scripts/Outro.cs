using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outro : MonoBehaviour
{
    public Animator gravityLine;
    public GameObject belly; //why do you keep disapearing D:

    // Start is called before the first frame update
    void Start()
    {
        belly.SetActive(true);
        StartCoroutine(GravityLineDelay());
    }

    IEnumerator GravityLineDelay()
    {
        yield return new WaitForSeconds(2);
        gravityLine.Play("OutroGravityLine");
    }
}
