using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private GameObject PausePanel;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private GameObject WinPanel;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Button[] buttons;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Button[] buttonsWin;

    public void Pause()
    {
        PausePanel.SetActive(true);
        buttons[0].Select();
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        buttonsWin[0].Select();
    }

    public void UnPause()
    {
        PausePanel.SetActive(false);
    }

    public void CustomSelectPause()
    {
        bool selected = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == buttons[i].gameObject)
            {
                selected = true;
            }
        }
        if (!selected)
        {
            buttons[0].Select();
        }
    }

    public void CustomSelectWin()
    {
        bool selected = false;
        for (int i = 0; i < buttonsWin.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == buttonsWin[i].gameObject)
            {
                selected = true;
            }
        }
        if (!selected)
        {
            buttonsWin[0].Select();
        }
    }
}
