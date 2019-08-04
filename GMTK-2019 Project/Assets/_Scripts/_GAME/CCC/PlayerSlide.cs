using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Say player can slide, or climb !")]
public class PlayerSlide : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), SerializeField, Tooltip("ref rigidbody")]
    private float dotMarginNiceSlope = 0.3f;
    [FoldoutGroup("GamePlay"), MinMaxSlider(0f, 1f), SerializeField, Tooltip("ref rigidbody")]
    private Vector2 minMaxMagnitude = new Vector2(0f, 0.7f);
    [FoldoutGroup("GamePlay"), SerializeField, Tooltip("ref rigidbody")]
    private float minMagnitudeSlideWhenCastRightOrLeft = 0.4f;

    [FoldoutGroup("Object"), SerializeField, Tooltip("ref rigidbody")]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("Object"), SerializeField, Tooltip("ref rigidbody")]
    private GroundForwardCheck _groundForwardCheck;

    [FoldoutGroup("Object"), SerializeField, Tooltip("ref rigidbody")]
    private Rigidbody rb = null;


    [FoldoutGroup("Debug"), ReadOnly, Tooltip("main Straff direction")]
    private Vector3 playerStraff = Vector3.zero;

    public Vector3 GetStraffDirection()
    {
        //Debug.Log("straff required for moving: " + playerStraff);
        return (playerStraff);
    }

    /// <summary>
    /// calculate the normal direction, based on the hit forward
    /// </summary>
    public void CalculateStraffDirection(Vector3 normalHit)
    {
        Vector3 playerDir = _playerLinker.PlayerInput.GetRelativeDirection();



        //Debug.Log("ici calcul straff, normal hit: " + normalHit);

        //Debug.DrawRay(rb.transform.position, playerDir * 50, Color.red, 0.5f);

        Vector3 upPlayer = Vector3.up;
        float dotWrongSide = ExtVector3.DotProduct(upPlayer, normalHit);

        //here the slope is nice for normal forward ?
        if (1 - dotWrongSide < dotMarginNiceSlope)
        {
            //Debug.Log("nice slope, do nothing: dot: " + dotWrongSide + "(max: " + dotMarginNiceSlope + ")");
            playerStraff = playerDir;
        }
        else
        {
            Vector3 relativeDirPlayer = _playerLinker.PlayerInput.GetRelativeDirection();
            float dotRight = 0f;
            float dotLeft = 0f;
            int rightOrLeft = ExtVector3.IsRightOrLeft(normalHit, upPlayer, relativeDirPlayer, rb.position, ref dotRight, ref dotLeft);

            Vector3 right = ExtVector3.CrossProduct(normalHit, upPlayer);

            if (rightOrLeft == 1)
            {
                playerStraff = ExtVector3.GetProjectionOfAOnB(relativeDirPlayer, right, upPlayer, minMaxMagnitude.x, minMaxMagnitude.y);// right * (dotRight);
                if (_groundForwardCheck.IsAdvancedForwardCastRightOrLeft())
                {
                    //Debug.LogWarning("zero ! warning, slide if not both " + playerStraff);
                    if (playerStraff.magnitude < minMagnitudeSlideWhenCastRightOrLeft)
                    {
                        playerStraff = playerStraff.normalized * minMagnitudeSlideWhenCastRightOrLeft;
                    }
                }
                //Debug.DrawRay(rb.position, playerStraff * 50, Color.magenta, 0.1f);
            }
            else if (rightOrLeft == -1)
            {
                playerStraff = ExtVector3.GetProjectionOfAOnB(relativeDirPlayer, - right, upPlayer, minMaxMagnitude.x, minMaxMagnitude.y);//-right * dotLeft;
                if (_groundForwardCheck.IsAdvancedForwardCastRightOrLeft())
                {
                    if (playerStraff.magnitude < minMagnitudeSlideWhenCastRightOrLeft)
                    {
                        playerStraff = playerStraff.normalized * minMagnitudeSlideWhenCastRightOrLeft;
                    }
                }

                //Debug.DrawRay(rb.position, playerStraff * 50, Color.magenta, 0.1f);
                //Debug.Log("ok left");
            }
            else
            {
                playerStraff = Vector3.zero;
            }
        }
        
    }
}
