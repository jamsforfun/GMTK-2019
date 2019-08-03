using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteraction : MonoBehaviour
{
    private Pickable _currentItem;
    public bool HasItem { get { return _currentItem != null; } }

    public void SetItem(Pickable newItem, out bool hasItemSwapped)
    {
        if (_currentItem == null)
        {
            _currentItem = newItem;
            hasItemSwapped = true;

        }
        else
        {
            _currentItem.DropItem();
            Debug.Log("Item dropped");
            _currentItem = newItem;
            hasItemSwapped = true;
        }
    }

    public void RemoveItem()
    {
        _currentItem = null;
    }

#if UNITY_EDITOR
    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.D) && _currentItem != null)
        {
            _currentItem.DropItem();
            _currentItem = null;
        } 
    }
#endif 
}
