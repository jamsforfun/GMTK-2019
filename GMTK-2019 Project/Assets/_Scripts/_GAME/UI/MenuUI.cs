using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Button[] buttons;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Button _creditsButton;

    public bool IsInCredits = false;

    public void SetInCredits(bool inCredits)
    {
        IsInCredits = inCredits;
    }

    private void Start()
    {
        buttons[0].Select();
    }

    private void Update()
    {
        if (IsInCredits)
        {
            _creditsButton.Select();
            return;
        }

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
}
