using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class AllCameras : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private List<CinemachineVirtualCamera> cams;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private CinemachineTargetGroup _targetGroup;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Transform _winTransform;

    private void ActiveCam(int index)
    {
        for (int i = 0; i < cams.Count; i++)
        {
            if (i == index)
            {
                cams[i].Priority = 10;
            }
            else
            {
                cams[i].Priority = 0;
            }
        }
    }

    public void ActiveMainCamera(List<Transform> allTargets)
    {
        //_targetGroup.m_Targets = new CinemachineTargetGroup.Target[allTargets.Count];
        for (int i = 0; i < allTargets.Count; i++)
        {
            _targetGroup.AddMember(allTargets[i], 1, 1);
        }
        ActiveCam(0);
    }

    public void DoorOpen()
    {
        ActiveCam(1);
        _targetGroup.AddMember(_winTransform, 3f, 1);
    }

    public void ActiveWinCam()
    {
        
        ActiveCam(2);
    }
}
