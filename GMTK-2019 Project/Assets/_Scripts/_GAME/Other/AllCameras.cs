using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class AllCameras : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private List<CinemachineVirtualCamera> cams;

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

    public void ActiveMainCamera()
    {
        ActiveCam(0);
    }

    public void ActiveWinCam()
    {
        ActiveCam(1);
    }
}
