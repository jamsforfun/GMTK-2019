using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInteraction : MonoBehaviour
{
    private Pickable _currentItem;
    public bool HasItem { get { return _currentItem != null; } }

    public void SetItem(Pickable newItem)
    {
        if (_currentItem == null)
        {
            _currentItem = newItem;

        }
        else
        {
            _currentItem.DropItem();
            _currentItem = newItem;
        }
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
