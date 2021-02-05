using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum LineType
{
    Normal,
    Ice,
    Rubber,
    Weight
}
public class GameControl : MonoBehaviour
{
    LineType lineType;
    public float SetMinDrawDistanceAroundPlayer;
    static public float MinDrawDistanceAroundPlayer;
    public GameObject GameCursor;
    public Sprite CursorNormal, CursorIce, CursorRubber, CursorWeight;
    public GameObject LinePrefab;
    public float LifeTimeAfterNewLine;
    public float DrawRateSeconds;
    public GameObject Player;
    GameObject lastLine;
    // Start is called before the first frame update
    void Start()
    {
        MinDrawDistanceAroundPlayer = SetMinDrawDistanceAroundPlayer;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(0);
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        Vector3 mousePos = Input.mousePosition;
        GameCursor.transform.position = new Vector3(mousePos.x + 18, mousePos.y - 20, mousePos.z);

        if (!Input.GetMouseButton(0))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                lineType = LineType.Normal;
                GameCursor.GetComponent<Image>().sprite = CursorNormal;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                lineType = LineType.Ice;
                GameCursor.GetComponent<Image>().sprite = CursorIce;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                lineType = LineType.Rubber;
                GameCursor.GetComponent<Image>().sprite = CursorRubber;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                lineType = LineType.Weight;
                GameCursor.GetComponent<Image>().sprite = CursorWeight;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Camera.main.transform.forward, 20f, 1 << 8);
            if (rayhit.collider != null) StartCoroutine(Dragging(rayhit.collider.gameObject));
            else
            {
                Vector2 lineStartPos = Camera.main.ScreenToWorldPoint(mousePos, Camera.MonoOrStereoscopicEye.Mono);
                Vector2 playerPos = Player.transform.position;
                if (Vector2.Distance(playerPos, lineStartPos) < MinDrawDistanceAroundPlayer)
                {
                    lineStartPos = ((lineStartPos - playerPos).normalized * MinDrawDistanceAroundPlayer) + playerPos; 
                }
                GameObject line = Instantiate(LinePrefab, lineStartPos, Quaternion.identity);
                if (lastLine != null) Destroy(lastLine, LifeTimeAfterNewLine);
                lastLine = line;
                line.GetComponent<Line>().ConstructFromCursor(DrawRateSeconds, lineType, true, Player);
            }
        }
    }

    IEnumerator Dragging(GameObject ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        float initG = 0;
        if (rb != null)
        {
            initG = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        while (Input.GetMouseButton(0))
        {
            Vector3 pos = ball.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            ball.transform.position = pos;
            yield return new WaitForEndOfFrame();
        }
        if (rb != null) rb.gravityScale = initG;
    }
}
