using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    public float speed;

    float timePassed;
    private Vector2 top;
    private Vector2 bot;
    // Update is called once per frame
    private void Start()
    {
        top = topPoint.position;
        bot = bottomPoint.position;
    }
    void Update()
    {
        timePassed += Time.deltaTime/speed;
        gameObject.transform.position = Vector2.Lerp(top, bot, Mathf.PingPong(timePassed, 1f));
    }
}
