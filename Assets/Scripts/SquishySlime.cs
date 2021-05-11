using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.U2D;

public class SquishySlime : MonoBehaviour
{

    [Tooltip("This number determines how squishy the slime is")]
    public float PointForce, PointAccuracy;
    public List<GameObject> Points = new List<GameObject>();
    public List<Transform> originPoints = new List<Transform>();
    public SpriteShapeController ShapeController;

    private void FixedUpdate()
    {

        for(int i = 0; i < Points.Count; i++)
        {

            Vector2 posDif = originPoints[i].position - Points[i].transform.position; Vector2 posDifNormal = posDif.normalized;
            if(posDif.magnitude < PointAccuracy)
                Points[i].transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            else
                Points[i].transform.GetComponent<Rigidbody2D>().velocity = Points[i].transform.GetComponent<Rigidbody2D>().velocity + (posDifNormal * PointForce);

            ShapeController.spline.SetPosition(i, Points[i].transform.position);

            if(i == 0)
            {

                ShapeController.spline.SetLeftTangent(i, -Points[5].transform.position);
                ShapeController.spline.SetRightTangent(i, -Points[1].transform.position);                

            }
            else if(i == 5)
            {

                ShapeController.spline.SetLeftTangent(i, -Points[4].transform.position);
                ShapeController.spline.SetRightTangent(i, -Points[0].transform.position);

            }
            else
            {

                ShapeController.spline.SetLeftTangent(i, -Points[i - 1].transform.position);
                ShapeController.spline.SetLeftTangent(i, -Points[i + 1].transform.position);

            }

        }

    }

}
