using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteraction : MonoBehaviour
{
    private Pickable _currentItem;
    public bool HasItem { get { return _currentItem != null; } }

    public void SetItem(Pickable newItem, Vector3 dropDirection, out bool hasItemSwapped)
    {
        if (_currentItem == null)
        {
            _currentItem = newItem;
            hasItemSwapped = true;

        }
        else
        {
            _currentItem.DropItem(dropDirection);
            Debug.Log("The item that drops " + _currentItem.gameObject.name);
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
            _currentItem.DropItem(transform.forward);
            _currentItem = null;
        } 
    }
#endif 
}
