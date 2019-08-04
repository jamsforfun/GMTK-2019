using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandfilOnPlayer : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    public bool IsInside = false;
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    public float TimeMaxInsidePlayer = 5f;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField, ReadOnly]
    private Landfill _refLandFil;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField, ReadOnly]
    private Pickable _pickablePrefabsTOCreate;

    private FrequencyCoolDown _timeInside = new FrequencyCoolDown();

    private bool isInit = false;

    /// <summary>
    /// called when we enter in a new land
    /// </summary>
    /// <param name="landfill"></param>
    public void SetNewLandFill(Landfill landfill, Pickable pickable)
    {
        _refLandFil = landfill;
        _pickablePrefabsTOCreate = pickable;
        IsInside = true;
        isInit = true;
        _timeInside.StartCoolDown(TimeMaxInsidePlayer);
        Debug.Log("enter !");
    }

    /// <summary>
    /// called when we leave a land
    /// </summary>
    /// <param name="landfill"></param>
    public void OutLandFill()
    {
        isInit = false;
        IsInside = false;
        _timeInside.Reset();
        Debug.Log("exit !");
        _refLandFil = null;
        _pickablePrefabsTOCreate = null;
    }

    /// <summary>
    /// called when we are inside, and the timer is over
    /// </summary>
    public void TimerEndInside()
    {
        if (_playerLinker.PlayerObjectInteraction.HasItem)
        {
            Debug.Log("timer end, but has already an item !");
            return;
        }

        Debug.Log("timer end !");
        Pickable.GivePickableToPlayer(_playerLinker, _pickablePrefabsTOCreate);
    }

    /// <summary>
    /// called if we are inside the landfil
    /// </summary>
    /// <param name="landfill"></param>
    public void SetInside(Landfill landfill, Pickable pickable)
    {
        if (_refLandFil == null ||
            ( _refLandFil.GetInstanceID() != landfill.GetInstanceID()
            && !_playerLinker.PlayerObjectInteraction.HasItem))
        {
            SetNewLandFill(landfill, pickable);
        }
        else
        {
            //here in the old one
            IsInside = true;
            isInit = true;

            if (_timeInside.IsStartedAndOver())
            {
                TimerEndInside();
            }
        }
        
    }

    public void TryToLeave()
    {
        if (isInit)
        {
            isInit = false;
            return;
        }
        //here we are not inside anymore
        if (IsInside)
        {
            OutLandFill();
        }
    }
}
