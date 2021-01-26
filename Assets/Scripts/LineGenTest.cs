using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenTest : MonoBehaviour
{
    public Transform a, b, c;
    public GameObject linePrefab;

    GameObject current;

    [Tooltip("Make the circle piece be drawn continously at the rate defined below")]
    public bool Continuous;
    [Tooltip("The rate at which to draw new circles when Continuous is checked")]
    public float drawRate;
    [Tooltip("The limit of line pieces in a circle, set to avoid infinite loops due to inprecise cosigns")]
    public int iterationLimit;

    bool drawing;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (Continuous)
            {
                if (drawing)
                {
                    StopCoroutine(Drawing());
                    drawing = false;
                }
                else
                {
                    StartCoroutine(Drawing());
                    drawing = true;
                }
            }
            else
            {
                if (current != null) Destroy(current);
                current = Instantiate(linePrefab, transform.position, Quaternion.identity);
                current.GetComponent<Line>().ConstructFromPoints(a.position, b.position, c.position, LineType.Normal, 0.5f, iterationLimit);
            }
        }
    }

    IEnumerator Drawing()
    {
        while(true)
        {
            if (current != null) Destroy(current);
            current = Instantiate(linePrefab, transform.position, Quaternion.identity);
            current.GetComponent<Line>().ConstructFromPoints(a.position, b.position, c.position, LineType.Normal, 0.5f, iterationLimit);
            yield return new WaitForSeconds(drawRate);
        }
    }
}
