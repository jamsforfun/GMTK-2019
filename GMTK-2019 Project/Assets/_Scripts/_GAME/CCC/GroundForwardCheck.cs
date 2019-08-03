using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundForwardCheck : MonoBehaviour
{
    public enum AdvancedForwardType
    {
        LEFT = -1,
        RIGHT_AND_LEFT = 0,
        RIGHT = 1,
        NONE = 2,
    }

    [FoldoutGroup("GamePlay"), Range(0f, 1f), Tooltip(""), SerializeField]
    public float timeBetween2TestForward = 0.8f;

    [FoldoutGroup("Forward"), Range(0f, 2f), Tooltip("dist to check forward player"), SerializeField]
    private float distForward = 0.6f;
    [FoldoutGroup("Forward"), Tooltip(""), SerializeField]
    public float sizeRadiusForward = 0.3f;
    [FoldoutGroup("Forward"), Range(0f, 1f), Tooltip(""), SerializeField]
    public float dotMarginImpact = 0.3f;
    

    [FoldoutGroup("Advance Forward"), Tooltip("rigidbody"), SerializeField]
    private float upDistRaycast = 0.1f;
    [FoldoutGroup("Advance Forward"), Tooltip("rigidbody"), SerializeField]
    private float lateralDistRaycast = 0.3f;
    [FoldoutGroup("Advance Forward"), Tooltip("rigidbody"), SerializeField]
    private float distForwardRaycast = 1f;

    [FoldoutGroup("Advance Forward"), ReadOnly, SerializeField]
    private bool isAdvancedForward = false;
    [FoldoutGroup("Advance Forward"), ReadOnly, SerializeField]
    private AdvancedForwardType isForwardAdvanceRightOrLeft = AdvancedForwardType.NONE;
    public bool IsAdvancedForwardCastRightOrLeft()
    {
        return (isForwardAdvanceRightOrLeft == AdvancedForwardType.RIGHT
            || isForwardAdvanceRightOrLeft == AdvancedForwardType.LEFT);
    }

    public int layerMask = Physics.AllLayers;

    //[FoldoutGroup("Backward"), Range(0f, 2f), Tooltip("dist to check forward player"), SerializeField]
    //private float distBackward = 1f;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private PlayerSlide _playerSlide = null;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private PlayerLinker _playerLinker;
    /*
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private GroundCheck groundCheck = null;
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private Rigidbody rb = null;
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private EntityController entityController = null;
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private EntityGravity entityGravity = null;
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private EntityAction entityAction = null;

[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private EntityGravityAttractorSwitch entityGravityAttractorSwitch = null;
[FoldoutGroup("Object"), Tooltip(""), SerializeField]
private EntityBumpUp entityBumpUp = null;
*/

    
    //[FoldoutGroup("Debug"), ReadOnly, SerializeField]
    //private bool isBackwardWall = false;
    [FoldoutGroup("Debug"), Tooltip("reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)"), SerializeField]
    public float collRayCastMargin = 0.1f;


    private FrequencyCoolDown coolDownForward = new FrequencyCoolDown();
    private Vector3 dirSurfaceNormal;

    public bool IsCoolDownSwitchReady()
    {
        return (coolDownForward.IsReady());
    }

    public bool IsStraffNeeded()
    {
        return (isForwardAdvanceRightOrLeft != AdvancedForwardType.NONE);
    }

    private void AdvanceForwardCheck()
    {
        isAdvancedForward = false;
        isForwardAdvanceRightOrLeft = AdvancedForwardType.NONE;

        RaycastHit hitLeft;
        RaycastHit hitRight;
        Vector3 origin = _playerLinker.Rigidbody.transform.position + Vector3.down * upDistRaycast;
        Vector3 originRight = origin + _playerLinker.PlayerManager.GetFocusedRightDirPlayer() * lateralDistRaycast;
        Vector3 originLeft = origin - _playerLinker.PlayerManager.GetFocusedRightDirPlayer() * lateralDistRaycast;
        Vector3 dirRaycast = _playerLinker.PlayerManager.GetFocusedForwardDirPlayer();

        Debug.DrawRay(origin, dirRaycast, Color.magenta);
        Debug.DrawRay(originRight, dirRaycast, Color.magenta);
        Debug.DrawRay(originLeft, dirRaycast, Color.magenta);

        if (Physics.Raycast(originRight, dirRaycast, out hitRight, distForwardRaycast, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("Did Hit: " + hitRight.collider.gameObject, hitRight.collider.gameObject);
            isAdvancedForward = true;
  //isForwardAdvanceNormalOk = IsNormalOk(hitRight);
            isForwardAdvanceRightOrLeft = AdvancedForwardType.RIGHT;
            _playerSlide.CalculateStraffDirection(hitRight.normal);    //calculate SLIDE
            
            //return (true);
        }
        if (Physics.Raycast(originLeft, dirRaycast, out hitLeft, distForwardRaycast, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("Did Hit: " + hitLeft.collider.gameObject, hitLeft.collider.gameObject);
            isAdvancedForward = true;
  //isForwardAdvanceNormalOk = IsNormalOk(hitLeft);

            isForwardAdvanceRightOrLeft = (isForwardAdvanceRightOrLeft == AdvancedForwardType.NONE) ? AdvancedForwardType.LEFT : AdvancedForwardType.RIGHT_AND_LEFT;
            _playerSlide.CalculateStraffDirection(hitLeft.normal);    //calculate SLIDE            
        }
    }

    /*
    public bool IsNormalOk(RaycastHit hitInfo)
    {
        if (entityController.IsForbidenLayerSwitch(LayerMask.LayerToName(hitInfo.transform.gameObject.layer))
                || (entityController.IsMarioGalaxyPlatform(LayerMask.LayerToName(hitInfo.collider.gameObject.layer)))
                    && !entityGravityAttractorSwitch.IsNormalIsOkWithCurrentGravity(hitInfo.normal, entityGravityAttractorSwitch.GetGAGravityAtThisPoint(hitInfo.point)))
        {
            //here we are in front of a forbidden wall !!
            return (false);
        }
        return (true);
    }
    */

        /*
    private void ForwardWallCheck()
    {
        RaycastHit hitInfo;

        ResetContact();
        isAdvancedForward = false;

        //do nothing if not moving
        if (!_playerLinker.PlayerInput.IsMoving())
            return;

        AdvanceForwardCheck();

        
        if (Physics.SphereCast(rb.transform.position, sizeRadiusForward, entityController.GetFocusedForwardDirPlayer(), out hitInfo,
                               distForward, entityController.layerMask, QueryTriggerInteraction.Ignore))
        {
            //if (!IsSphereGravityAndNormalNotOk(hitInfo))
            //    return;

            
            //ExtDrawGuizmos.DebugWireSphere(rb.transform.position + (entityController.GetFocusedForwardDirPlayer()) * (distForward), Color.yellow, sizeRadiusForward, 0.1f);
            //Debug.DrawRay(rb.transform.position, (entityController.GetFocusedForwardDirPlayer()) * (distForward), Color.yellow, 5f);
            //ExtDrawGuizmos.DebugWireSphere(hitInfo.point, Color.red, 0.1f, 0.1f);

            isForwardWall = true;

            Vector3 normalHit = hitInfo.normal;
            Vector3 upPlayer = entityGravity.GetMainAndOnlyGravity();
            Vector3 tmpDirSurfaceNormal = ExtPhysics.GetSurfaceNormal(rb.transform.position,
                               entityController.GetFocusedForwardDirPlayer(),
                               distForward,
                               sizeRadiusForward,
                               hitInfo.point,
                               collRayCastMargin,
                               entityController.layerMask);
            if (tmpDirSurfaceNormal != Vector3.zero)
                dirSurfaceNormal = tmpDirSurfaceNormal;

            _playerSlide.CalculateStraffDirection(dirSurfaceNormal);    //calculate SLIDE

            float dotWrongSide = ExtVector3.DotProduct(upPlayer, normalHit);
            if (dotWrongSide < -dotMarginImpact)
            {
                //Debug.Log("forward too inclined, dotImpact: " + dotWrongSide + "( max: " + dotMarginImpact + ")");
                isForbiddenForward = true;
                return;
            }

            //int isForbidden = ExtList.ContainSubStringInArray(walkForbiddenForwardUp, LayerMask.LayerToName(hitInfo.transform.gameObject.layer));
            if (!IsNormalOk(hitInfo))
            {
                //here we are in front of a forbidden wall !!
                isForbiddenForward = true;
                
                entityBumpUp.HereBumpUp(hitInfo, dirSurfaceNormal);
            }
            else
            {
                if (groundCheck.IsFlying() && !inAirForwardWall)
                {
                    isForbiddenForward = true;
                }
                else
                {
                    //HERE FORWARD, DO SWITCH !!
                    coolDownForward.StartCoolDown(timeBetween2TestForward);
                    //Debug.Log("forward");
                    groundCheck.SetForwardWall(hitInfo);
                    
                    isForbiddenForward = false;
                }
            }
        }
        else
        {
            
            ResetContact();
        }
        
    }
    */
    
    

    private void ResetBackwardContact()
    {
//isBackwardWall = false;
//isForbiddenBackward = false;
    }

    private void FixedUpdate()
    {
        //set if the is a wall in front of us (we need flying info)
        AdvanceForwardCheck();
        //BackwardWallCheck();
    }
}
