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
    public static GameControl main {get; private set;}
    LineType lineType;
    public float SetMinDrawDistanceAroundPlayer;
    static public float MinDrawDistanceAroundPlayer;
    public GameObject GameCursor;
    public Sprite CursorNormal, CursorIce, CursorRubber, CursorWeight;
    public GameObject LinePrefab;
    public float LifeTimeAfterNewLine;
    public float DrawRateSeconds;
    public float StraightPieceLength;
    public GameObject Player;
    public long Coins;
    public int[] Ink;
    GameObject lastLine;

    bool AssistedDraw;
    public bool LimitLinesInAir;
    public int NormalLimit, IceLimit, RubberLimit, WeightLimit;
    private int normalLeft, iceLeft, rubberLeft, weightLeft;

    public void ResetLineLimits()
    {
        normalLeft = NormalLimit;
        iceLeft = IceLimit;
        rubberLeft = RubberLimit;
        weightLeft = WeightLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        MinDrawDistanceAroundPlayer = SetMinDrawDistanceAroundPlayer;
        Cursor.visible = false;
        normalLeft = NormalLimit;
        iceLeft = IceLimit;
        rubberLeft = RubberLimit;
        weightLeft = WeightLimit;
        Ink = new int[System.Enum.GetNames(typeof(LineType)).Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(0);
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        Vector3 mousePos = Input.mousePosition;
        GameCursor.transform.position = new Vector3(mousePos.x + 18, mousePos.y - 20, mousePos.z);

        if (Input.GetMouseButtonDown(1))
        {
            if (lastLine != null)
            {
                Destroy(lastLine);
                lastLine = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Camera.main.transform.forward, 20f, 1 << 8);
            if (rayhit.collider != null) StartCoroutine(Dragging(rayhit.collider.gameObject));
            else
            {
                if (LimitLinesInAir)
                {
                    if (lineType == LineType.Normal)
                    {
                        if (normalLeft <= 0) return;
                        else normalLeft--;
                    }
                    if (lineType == LineType.Ice)
                    {
                        if (iceLeft <= 0) return;
                        else iceLeft--;
                    }
                    if (lineType == LineType.Rubber)
                    {
                        if (rubberLeft <= 0) return;
                        else rubberLeft--;
                    }
                    if (lineType == LineType.Weight)
                    {
                        if (weightLeft <= 0) return;
                        else weightLeft--;
                    }
                }
                Vector2 lineStartPos = Camera.main.ScreenToWorldPoint(mousePos, Camera.MonoOrStereoscopicEye.Mono);
                Vector2 playerPos = Player.transform.position;
                if (Vector2.Distance(playerPos, lineStartPos) < MinDrawDistanceAroundPlayer)
                {
                    lineStartPos = ((lineStartPos - playerPos).normalized * MinDrawDistanceAroundPlayer) + playerPos;
                }
                GameObject line = Instantiate(LinePrefab, lineStartPos, Quaternion.identity);
                if (lastLine != null) Destroy(lastLine, LifeTimeAfterNewLine);
                lastLine = line;
                if (AssistedDraw) line.GetComponent<Line>().ConstructFromCursor(lineType, false, Player, DrawRateSeconds, StraightPieceLength);
                else line.GetComponent<Line>().ConstructFromCursor(lineType, true, Player, DrawRateSeconds, StraightPieceLength);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                AssistedDraw = !AssistedDraw;
            }
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
    }

    public void ModInk(LineType type, int amount)
    {
        Ink[(int)type] += amount;

        // Update UI for inkwells here
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
