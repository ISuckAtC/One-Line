using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    public Animator gravityLine;
    public GameObject belly, InputField; //why do you keep disapearing D:

    // Start is called before the first frame update
    void Start()
    {
        belly.SetActive(true);
        StartCoroutine(GravityLineDelay());
        StartCoroutine(LoadMenu());
    }

    IEnumerator GravityLineDelay()
    {
        yield return new WaitForSeconds(2);
        gravityLine.Play("OutroGravityLine");
    }
    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(35);

        InputField.SetActive(true);

    }
}
