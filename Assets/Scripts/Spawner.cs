using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IActivatable
{
    public GameObject Spawn;
    public int SpawnLimit;
    public int DespawnTime;
    private List<GameObject> lastObjects;

    public void Start()
    {
        lastObjects = new List<GameObject>();
    }
    public void Activate()
    {
        lastObjects.Add(Instantiate(Spawn, transform.position, Quaternion.identity));
        if (lastObjects.Count > SpawnLimit) 
        {
            StartCoroutine(Despawn(lastObjects[0]));
            lastObjects.RemoveAt(0);
        }
    }

    public IEnumerator Despawn(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color color = sr.material.color;
        for (int i = 0; i < DespawnTime; ++i)
        {
            yield return new WaitForFixedUpdate();
            color.a = Mathf.Lerp(1f, 0f, (1f / (float)DespawnTime) * (float)i);
            sr.material.color = color;
        }
        Destroy(obj);
    }
}
