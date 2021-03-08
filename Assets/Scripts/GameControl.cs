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
    Weight,
    Joint
}
public class GameControl : MonoBehaviour
{
    public static GameControl main { get; private set; }
    LineType lineType;
    public float SetMinDrawDistanceAroundPlayer;
    static public float MinDrawDistanceAroundPlayer;
    public GameObject GameCursor;
    public Sprite CursorNormal, CursorIce, CursorRubber, CursorWeight, CursorJoint, InkNormal, InkIce, InkRubber, InkWeight, InkJoint;
    public GameObject LinePrefab;
    public float LifeTimeAfterNewLine;
    public float DrawRateSeconds;
    public float StraightPieceLength;
    public float MinLineLength;
    public GameObject Player;
    public bool UseInk;
    public bool InkByLength;
    [Tooltip("If empty, creates empty inkwell. If values are specified make sure the size is equal to the amount of line types")]
    public int[] Ink;
    GameObject lastLine;
    public int InkTypeSelected;

    bool AssistedDraw;
    public bool LimitLinesInAir;
    public int NormalLimit, IceLimit, RubberLimit, WeightLimit, JointLimit;
    private int normalLeft, iceLeft, rubberLeft, weightLeft, jointLeft;
    //private Text[] inkWellTexts;

    public float LevelTransCamOffset;
    public float LevelStartCamDelay;
    public float LevelEndCamDelay;
    public float CamFollowSpeed;
    public float DrawingTimeScale;

    public LineType DefaultLineType;
    public bool ForceDefault;
    public bool SwitchOnInkEmpty;
    static private LineType? lastType;
    public Dialogue dialogue;
    public GameObject CursorLinePanel;
    public Text CursorLineText;
    public char CustomWaitDefCharacter;
    public int CustomWaitDefDigits;
    public bool LevelCompleted;
    public GlobalData Global;

    public void ResetLineLimits()
    {
        normalLeft = NormalLimit;
        iceLeft = IceLimit;
        rubberLeft = RubberLimit;
        weightLeft = WeightLimit;
        //jointLeft = JointLimit;
    }

    void Awake()
    {
        if (Global == null) 
        {
            Global = (new GameObject("Global Data")).AddComponent<GlobalData>();
            DontDestroyOnLoad(Global.gameObject);
            Debug.Log(Global.Coins);
        }
        CursorLinePanel.SetActive(false);
        main = this;
        if (ForceDefault) lineType = DefaultLineType;
        else
        {
            if (lastType != null) lineType = (LineType)lastType;
            else lineType = DefaultLineType;
        }
        lastType = null;
        StartCoroutine(StartTravel());
    }

    // Start is called before the first frame update
    void Start()
    {
        MinDrawDistanceAroundPlayer = SetMinDrawDistanceAroundPlayer;
        Cursor.visible = false;

        normalLeft = NormalLimit;
        iceLeft = IceLimit;
        rubberLeft = RubberLimit;
        weightLeft = WeightLimit;
        //jointLeft = JointLimit;

        if (Ink.Length < 4 /*System.Enum.GetNames(typeof(LineType)).Length*/) Ink = new int[System.Enum.GetNames(typeof(LineType)).Length];

        //inkWellTexts = new Text[System.Enum.GetNames(typeof(LineType)).Length];
        //inkWellTexts[0] = GameObject.Find("Text_Inkwell_Regular").GetComponent<Text>();
        //inkWellTexts[1] = GameObject.Find("Text_Inkwell_Ice").GetComponent<Text>();
        //inkWellTexts[2] = GameObject.Find("Text_Inkwell_Rubber").GetComponent<Text>();
        //inkWellTexts[3] = GameObject.Find("Text_Inkwell_Gravity").GetComponent<Text>();
        //inkWellTexts[4] = GameObject.Find("Text_Inkwell_Gravity").GetComponent<Text>();
        //for (int i = 0; i < Ink.Length; ++i) inkWellTexts[i].text = Ink[i].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) StartCoroutine(dialogue.Speak());
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (!LevelCompleted) Global.ResetCount++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        Vector3 mousePos = Input.mousePosition;
        //GameCursor.transform.position = new Vector3(mousePos.x + 18, mousePos.y - 20, mousePos.z);
        GameCursor.transform.position = new Vector3(mousePos.x, mousePos.y, mousePos.z);

        if (Input.GetMouseButtonDown(1))
        {
            if (lastLine != null)
            {
                Destroy(lastLine);
                lastLine = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //bit sloppy
            Time.timeScale = 1;
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
                    if (lineType == LineType.Joint)
                    {
                        if (jointLeft <= 0) return;
                        else jointLeft--;
                    }
                }
                if (UseInk)
                {
                    if (Ink[(int)lineType] <= 0) return;
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
                if (AssistedDraw) line.GetComponent<Line>().ConstructFromCursor(lineType, DrawingTimeScale, false, Player, DrawRateSeconds, StraightPieceLength);
                else line.GetComponent<Line>().ConstructFromCursor(lineType, DrawingTimeScale, true, Player, DrawRateSeconds, StraightPieceLength);
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
                SwitchLineType(LineType.Normal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchLineType(LineType.Ice);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchLineType(LineType.Rubber);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchLineType(LineType.Weight);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SwitchLineType(LineType.Joint);
            }
        }
    }

    public void SwitchLineType(LineType type)
    {
        lineType = type;
        switch (type)
        {
            case LineType.Normal:
                GameCursor.GetComponent<Image>().sprite = CursorNormal;
                InkTypeSelected = 0;
                break;
            case LineType.Ice:
                GameCursor.GetComponent<Image>().sprite = CursorIce;
                InkTypeSelected = 1;
                break;
            case LineType.Rubber:
                GameCursor.GetComponent<Image>().sprite = CursorRubber;
                InkTypeSelected = 2;
                break;
            case LineType.Weight:
                GameCursor.GetComponent<Image>().sprite = CursorWeight;
                InkTypeSelected = 3;
                break;
            case LineType.Joint:
                break;
        }
    }

    public void ModInk(LineType type, int amount)
    {
        Ink[(int)type] += amount;

        if (SwitchOnInkEmpty && Ink[(int)type] <= 0)
        {
            for (int i = 0; i < Ink.Length; ++i)
            {
                if (Ink[i] > 0)
                {
                    SwitchLineType((LineType)i);
                    break;
                }
            }
        }

        // Update UI for inkwells here

        //inkWellTexts[(int)type].text = Ink[(int)type].ToString();
    }
    public void ModInkDisplayOnly(LineType type, int setamount)
    {
        CursorLineText.text = (Ink[(int)type] + setamount).ToString();
        //inkWellTexts[(int)type].text = (Ink[(int)type] + setamount).ToString();
    }
    IEnumerator StartTravel()
    {
        Camera.main.transform.parent = null;
        Camera.main.transform.Translate(new Vector3(-LevelTransCamOffset, 0, 0));
        yield return new WaitForSeconds(LevelStartCamDelay);
        while (true)
        {
            Vector3 nPos = Vector2.MoveTowards(Camera.main.transform.position, Player.transform.position, CamFollowSpeed);
            nPos.z = Camera.main.transform.position.z;
            Camera.main.transform.position = nPos;
            if ((Vector2)Camera.main.transform.position == (Vector2)Player.transform.position) break;
            yield return new WaitForFixedUpdate();
        }
        Camera.main.transform.parent = Player.transform;
    }
    public IEnumerator EndTravel(string nextScene)
    {
        Camera.main.transform.parent = null;
        Vector3 nPos = Camera.main.transform.position;
        nPos.x = nPos.x + LevelTransCamOffset;
        yield return new WaitForSeconds(LevelEndCamDelay);
        while (true)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, nPos, CamFollowSpeed);
            if (Camera.main.transform.position == nPos) break;
            yield return new WaitForFixedUpdate();
        }
        lastType = lineType;
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
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
