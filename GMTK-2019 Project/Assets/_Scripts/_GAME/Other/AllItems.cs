using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AllItems : MonoBehaviour
{
    [FoldoutGroup("Object"), SerializeField]
    private AllPlayerLinker _allPlayerLinker;
    [FoldoutGroup("Object"), SerializeField]
    private Transform _parentOfAllItems;
    [FoldoutGroup("Object")]
    public List<Pickable> AllPickable = new List<Pickable>();

    private void SetChildAutomaticly()
    {
        if (_parentOfAllItems.childCount != AllPickable.Count)
        {
            AllPickable.Clear();
            for (int i = 0; i < _parentOfAllItems.childCount; i++)
            {
                AllPickable.Add(_parentOfAllItems.GetChild(i).GetComponent<Pickable>());
            }
        }
    }

    private void SetInEditorItems()
    {
        for (int i = 0; i < AllPickable.Count; i++)
        {
            AllPickable[i].AllItems = _parentOfAllItems;
            AllPickable[i].AllPlayerLinker = _allPlayerLinker;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {

        SetChildAutomaticly();
        SetInEditorItems();
    }
#endif

}
