using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandfilOnPlayer : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    public bool IsInside = false;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField, ReadOnly]
    private Landfill _refLandFil;

    private bool isInit = false;

    public void SetNewLandFill(Landfill landfill)
    {
        _refLandFil = landfill;

    }

    public void OutLandFil(Landfill landfill)
    {

        IsInside = false;
    }

    /// <summary>
    /// called if we are inside the landfil
    /// </summary>
    /// <param name="landfill"></param>
    public void SetInside(Landfill landfill)
    {
        if (_refLandFil == null || _refLandFil != landfill)
        {
            SetNewLandFill(landfill);
        }
        IsInside = true;
        isInit = true;
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
            OutLandFil(_refLandFil);
        }
    }
}
