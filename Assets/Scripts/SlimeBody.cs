using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class SlimeBody : MonoBehaviour
{
    #region Constants
    private const float splineOffset = 0.25f;
    #endregion

    #region Fields
    [SerializeField]
    public SpriteShapeController slimeShape;
    [SerializeField]
    public Transform[] points;
    #endregion

    #region Callbacks
    private void Awake()
    {
        UpdateVerts();
    }

    private void FixedUpdate()
    {
        UpdateVerts();
    }
    #endregion

    #region Methods
    private void UpdateVerts()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 _vert = points[i].localPosition;
            Vector2 _towardsCenter = (Vector2.zero - _vert).normalized;
            float _colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try
            {
                //try to set the vertex of the spriteshape at the approprate point
                slimeShape.spline.SetPosition(i, (_vert - _towardsCenter * _colliderRadius));
            }
            catch
            {
            
                //if they're too close 
                slimeShape.spline.SetPosition(i, (_vert - _towardsCenter * (_colliderRadius + splineOffset)));
            }

            //get a reference rotation of each vert
            Vector2 _LTan = slimeShape.spline.GetLeftTangent(i);

            //get each vert's smooth rotation
            Vector2 _newRTan = Vector2.Perpendicular(_towardsCenter) * _LTan.magnitude;
            Vector2 _newLTan = Vector2.zero - (_newRTan);

            //set the new rotations
            slimeShape.spline.SetRightTangent(i, _newRTan);
            slimeShape.spline.SetLeftTangent(i, _newLTan);

        }
    }
    #endregion
}
//Created with help of the YouTube-tutorial, "Unity SoftBody 2D tutorial using sprite shape", made by the channel LoneX
//https://www.youtube.com/watch?v=F82BlnW5z6g