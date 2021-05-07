using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTrigger : MonoBehaviour
{

    public GameObject[] Imp;

    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.tag == "Player")
        {
            for (int i =0; i<= Imp.Length; i++)
            {
                Imp[i].GetComponent<WizardBossBehaviour>().WizardBossActivate();
            }

        }

    }

}
