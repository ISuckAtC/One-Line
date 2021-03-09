using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS1718

public class Line : MonoBehaviour
{
    public PhysicsMaterial2D Bounce, Slide;
    public float NormalLength, IceLength, RubberLength, WeightLength, JointLength;
    public LineType LineType;
    PhysicsMaterial2D material;
    float lengthLimit;
    Color color;
    Vector2 End;
    public Sprite Circle;
    public Sprite Box;
    public float Thickness;
    public LinePiece[] Pieces { get { return pieces.ToArray(); } }
    public List<LinePiece> pieces;
    public List<LinePiece> stuckPieces;
    public int Size { get { return Pieces.Length; } }

    public float Length;
    public float MinLengthForNewPiece;
    public float MaxDistanceForValidConstruction;
    public float ThresholdAltConstruction;

    public GameObject LastPiece;
    public bool Refund;

    public void SetTriggerActive(bool active)
    {
        foreach (LinePiece piece in pieces) piece.SetTriggerActive(active);
    }

    public void ConstructFromPoints(Vector2 a, Vector2 b, Vector2 c, LineType lineType, float pieceLength, int iterationLimit)
    {
        Setup(lineType);
        lengthLimit = 500;

        Vector2 ac = c + ((a - c) / 2);
        End = ac;
        Vector2 acDir = Rotate((c - a).normalized, Mathf.Deg2Rad * 90f);


        float yRatioA = XtoYRatio(acDir);

        float hitYAxisA;

        hitYAxisA = ac.y - (yRatioA * ac.x);

        Debug.Log("A: " + ac + " | " + hitYAxisA + " | " + yRatioA);

        Vector2 bc = c + ((b - c) / 2);
        End = bc;
        Vector2 bcDir = Rotate((b - c).normalized, Mathf.Deg2Rad * 90f);

        /*
        for(int i = 0; i < 50; ++i)
        {
            Add(End + acDir, false);
        }
        for(int i = 0; i < 50; ++i)
        {
            Add(End + bcDir, false);
        }
        */


        float yRatioB = XtoYRatio(bcDir);

        float hitYAxisB;

        hitYAxisB = bc.y - (yRatioB * bc.x);

        Debug.Log("B: " + bc + " | " + hitYAxisB + " | " + yRatioB);

        float x = (hitYAxisA - hitYAxisB) * (1 / (yRatioB - yRatioA));
        Vector2 center = new Vector2(x, (yRatioA * x) + hitYAxisA);

        Debug.Log(center);


        GameObject xy = new GameObject();
        xy.transform.position = center;
        xy.transform.parent = transform;
        xy.name = "cross";


        float radius = Vector2.Distance(ac, center);

        Debug.Log(radius);

        End = a;

        int i = 0;

        while (End != b)
        {
            if (Vector2.Distance(End, b) < pieceLength)
            {
                Add(b, false);
                break;
            }
            float angle = Mathf.Acos(((Mathf.Pow(radius, 2) * 2) - Mathf.Pow(pieceLength, 2)) / (2 * radius * radius));
            Debug.Log(angle);
            Add(RotateAround(End, center, -angle), false);
            if (++i > iterationLimit)
            {
                Debug.Log("repeat limit reached");
                Debug.Log(Vector2.Distance(End, b));
                break;
            }
        }

        // check length against distance here
    }

    Vector2 Rotate(Vector2 a, float radians)
    {
        float cs = Mathf.Cos(radians);
        float sn = Mathf.Sin(radians);
        return new Vector2((a.x * cs) - (a.y * sn), (a.x * sn) + (a.y * cs));
    }

    Vector2 RotateAround(Vector2 start, Vector2 origin, float radians)
    {
        start -= origin;
        start = Rotate(start, radians);
        return start + origin;
    }

    float XtoYRatio(Vector2 a)
    {
        return a.y * (1 / a.x);
    }

    public void ConstructFromCursor(LineType lineType, float timeScale = 0.01f, bool freeDraw = false, GameObject player = null, float drawRate = 0, float pieceLength = 0)
    {
        Setup(lineType);
        Time.timeScale = timeScale;
        if (freeDraw) StartCoroutine(Drawing(drawRate, pieceLength, player));
        else StartCoroutine(DrawStraight(drawRate, pieceLength, player));
    }

    void Setup(LineType lineType)
    {
        End = transform.position;
        pieces = new List<LinePiece>();
        stuckPieces = new List<LinePiece>();
        LineType = lineType;
        switch (lineType)
        {
            case LineType.Normal:
                material = null;
                color = Color.white;
                lengthLimit = NormalLength;
                break;
            case LineType.Ice:
                material = Slide;
                color = Color.cyan;
                lengthLimit = IceLength;
                break;
            case LineType.Rubber:
                material = Bounce;
                color = Color.black;
                lengthLimit = RubberLength;
                break;
            case LineType.Weight:
                material = null;
                color = Color.gray;
                lengthLimit = WeightLength;
                break;
            case LineType.Joint:
                material = null;
                color = Color.yellow;
                lengthLimit = JointLength;
                break;
        }
    }

    public bool Add(Vector2 position, bool start, GameObject player = null, bool joint = false)
    {
        Vector2 playerPos = player.transform.position;
        string debugString = "Initial Position: (" + position + ") | ";
        if (player && Vector2.Distance(playerPos, position) < GameControl.MinDrawDistanceAroundPlayer)
        {
            Debug.Log("Around player");
            Vector2 newPosition = ((position - playerPos).normalized * GameControl.MinDrawDistanceAroundPlayer) + playerPos;
            if (Vector2.Distance(End, newPosition) > Vector2.Distance(End, position))
            {
                position = Vector2.Lerp(End, position, 0.5f);
                position = ((position - playerPos).normalized * GameControl.MinDrawDistanceAroundPlayer) + playerPos;
            }
            else position = newPosition;
        }

        debugString += "Post push position: (" + position + ") | ";

        if (Length + Vector2.Distance(End, position) > lengthLimit)
        {
            Debug.Log("Limit reached");
            position = Vector2.Lerp(End, position, (lengthLimit - Length) / Vector2.Distance(End, position));
        }

        debugString += "Post line limit position: (" + position + ") | ";

        if (GameControl.main.InkByLength && Length + Vector2.Distance(End, position) >= GameControl.main.Ink[(int)LineType])
        {
            Debug.Log("Total Limit reached");
            position = Vector2.Lerp(End, position, (GameControl.main.Ink[(int)LineType] - Length) / Vector2.Distance(End, position));
        }

        debugString += "Post total limit position: (" + position + ")";

        if (position.x != position.x)
        {
            Debug.LogError("NaN error in Line. End: (" + End + ") | Length: (" + Length + ") | " + debugString);
        }


        if (Vector2.Distance(End, position) < MinLengthForNewPiece && !start)
        {
            return true;
        }

        Length += Vector2.Distance(End, position);
        if (GameControl.main.InkByLength) 
        {
            GameControl.main.ModInkDisplayOnly(LineType, -(int)(Length + 1));
        }


        pieces.Add(new LinePiece(End, position, Thickness, Circle, Box, transform, this, material, color, joint));
        End = position;

        if (Length == lengthLimit)
        {
            return true;
            // Change the end of the last linepiece to an ending sprite
        }
        return false;
    }

    public void FromTo(Vector2 from, Vector2 to, float pieceLength, GameObject player = null, bool joint = false)
    {
        float length = Vector2.Distance(from, to);
        int pieceCount = (int)(length / pieceLength);
        Debug.Log("FromTo: Constructing " + pieceCount + " pieces");
        for (int i = 0; i < pieceCount; ++i)
        {
            if (Add(Vector2.Lerp(from, to, 1f / pieceCount * i), false, player, joint)) 
            {
                Debug.Log("Stopping early at limit");
                break;
            }
        }
    }

    public IEnumerator DrawStraight(float drawRate, float pieceLength, GameObject player = null)
    {
        if (GameControl.main.InkByLength) GameControl.main.CursorLinePanel.SetActive(true);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        Vector3 startPos = transform.position;
        startPos.z = 0;
        Add(pos, true, player);
        while (Input.GetMouseButton(0)) 
        {
            if (GameControl.main.InCutScene) break;
            if (GameControl.main.InkByLength) GameControl.main.ModInkDisplayOnly(LineType, (int)Vector2.Distance(transform.position, Input.mousePosition));
            yield return new WaitForSecondsRealtime(drawRate);
        }

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        float length = Vector2.Distance(startPos, pos);
        if (length > lengthLimit)
        {
            pos = Vector2.Lerp(startPos, pos, lengthLimit / length);
            length = Vector2.Distance(startPos, pos);
        }

        FromTo(startPos, pos, pieceLength, player);

        if (LineType == LineType.Weight)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 10;
        }
        if (!GameControl.main.InCutScene) Time.timeScale = 1;
        if (GameControl.main.InkByLength) GameControl.main.CursorLinePanel.SetActive(false);
        if (Length < GameControl.main.MinLineLength && Refund)
        {
            GameControl.main.ModInk(LineType, 0);
            Destroy(gameObject);
        } 
        else GameControl.main.ModInk(LineType, GameControl.main.InkByLength ? -(int)(Length + 1) : -1);
    }

    public IEnumerator Drawing(float drawRate, float pieceLength, GameObject player = null)
    {
        if (GameControl.main.InkByLength) GameControl.main.CursorLinePanel.SetActive(true);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        Add(pos, true, player, LineType == LineType.Joint);
        while (Input.GetMouseButton(0))
        {
            if (GameControl.main.InCutScene) break;
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            if (Vector2.Distance(End, pos) > ThresholdAltConstruction) FromTo(End, pos, pieceLength, player, LineType == LineType.Joint);
            else Add(pos, false, player, LineType == LineType.Joint);
            yield return new WaitForSecondsRealtime(drawRate);
        }
        if (LineType == LineType.Weight)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 10;
        }



        if (!GameControl.main.InCutScene) Time.timeScale = 1;
        if (GameControl.main.InkByLength) GameControl.main.CursorLinePanel.SetActive(false);
        if (Length < GameControl.main.MinLineLength && Refund)
        {
            GameControl.main.ModInk(LineType, 0);
            Destroy(gameObject);
        } 
        else GameControl.main.ModInk(LineType, GameControl.main.InkByLength ? -(int)(Length + 1) : -1);
    }
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

            // Change the end of the last linepiece to an ending sprite


            StopCoroutine(nameof(Drawing));
        }
    }
}

public class LinePiece
{
    GameObject StartCircle;
    GameObject MiddleBox;
    GameObject EndCircle;

    public void SetTriggerActive(bool active)
    {
        if (StartCircle) StartCircle.GetComponent<Collider2D>().isTrigger = active;
        if (MiddleBox) MiddleBox.GetComponent<Collider2D>().isTrigger = active;
        if (EndCircle) EndCircle.GetComponent<Collider2D>().isTrigger = active;
    }

    public LinePiece(Vector2 start, Vector2 end, float thickness, Sprite c, Sprite b, Transform parent, Line line, PhysicsMaterial2D mat, Color color, bool joint)
    {
        MiddleBox = new GameObject();
        SpriteRenderer mbr = MiddleBox.AddComponent<SpriteRenderer>();
        mbr.sprite = b;
        mbr.color = color;
        MiddleBox.layer = LayerMask.NameToLayer("Line");
        EndCircle = new GameObject();
        SpriteRenderer ecr = EndCircle.AddComponent<SpriteRenderer>();
        ecr.sprite = c;
        ecr.color = color;
        EndCircle.layer = LayerMask.NameToLayer("Line");

        if (start == end)
        {
            StartCircle = new GameObject("Start");
            SpriteRenderer scr = StartCircle.AddComponent<SpriteRenderer>();
            scr.sprite = c;
            scr.color = color;
            StartCircle.transform.position = start;
            StartCircle.transform.localScale = new Vector3(thickness, thickness, 1);
            CircleCollider2D ccs = StartCircle.AddComponent<CircleCollider2D>();
            StartCircle.transform.up = start - end;
            ccs.sharedMaterial = mat;
            StartCircle.transform.parent = parent;
            EndCircle.transform.up = -StartCircle.transform.up;

            StartCircle.layer = LayerMask.NameToLayer("Line");

            line.LastPiece = StartCircle;
            if (joint)
            {
                Debug.Log("Creating starting joint");
                Rigidbody2D rb = StartCircle.AddComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }
        else
        {
            if (line.stuckPieces.Count > 0)
            {
                foreach (LinePiece p in line.stuckPieces)
                {
                    p.StartCircle.transform.up = start - end;
                    p.MiddleBox.transform.up = end - start;
                    p.EndCircle.transform.up = end - start;
                }
                line.stuckPieces.Clear();
            }
            StartCircle = null;
            EndCircle.transform.up = end - start;
        }

        EndCircle.transform.position = end;
        EndCircle.transform.localScale = new Vector3(thickness, thickness, 1);

        float length = Vector2.Distance(start, end);
        MiddleBox.transform.localScale = new Vector3(thickness, length, 1);
        MiddleBox.transform.position = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);
        MiddleBox.transform.up = end - start;


        BoxCollider2D bc = MiddleBox.AddComponent<BoxCollider2D>();
        bc.sharedMaterial = mat;
        CircleCollider2D cce = EndCircle.AddComponent<CircleCollider2D>();
        cce.sharedMaterial = mat;

        MiddleBox.transform.parent = parent;
        EndCircle.transform.parent = parent;

        if (joint)
        {
            MiddleBox.AddComponent<Rigidbody2D>();
            FixedJoint2D fixedJoint2D = MiddleBox.AddComponent<FixedJoint2D>();
            Debug.Log(line.LastPiece.name);
            fixedJoint2D.connectedBody = line.LastPiece.GetComponent<Rigidbody2D>();
            fixedJoint2D.dampingRatio = 1;
            //fixedJoint2D.connectedAnchor = line.LastPiece.transform.position;

            EndCircle.AddComponent<Rigidbody2D>();
            HingeJoint2D hingeJoint2D = EndCircle.AddComponent<HingeJoint2D>();
            hingeJoint2D.connectedBody = line.LastPiece.GetComponent<Rigidbody2D>();
            //hingeJoint2D.connectedAnchor = line.LastPiece.transform.position;
        }

        line.LastPiece = EndCircle;

        if (start == end) line.stuckPieces.Add(this);
    }
}
