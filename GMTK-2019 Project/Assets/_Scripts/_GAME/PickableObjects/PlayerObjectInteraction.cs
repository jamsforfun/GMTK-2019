using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerObjectInteraction : MonoBehaviour
{
    [SerializeField] private GameState _gameState = default;
    private Pickable _currentItem;
    public bool HasItem { get { return _currentItem != null; } }

    private void Start()
    {
        if (_gameState == null)
        {
            _gameState = ExtUtilityFunction.GetScript<GameState>();
        }
    }

    public void SetItem(Pickable newItem, Vector3 dropDirection, out bool hasItemSwapped)
    {
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            hasItemSwapped = false;
            return;
        }

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
        if (!Application.isPlaying)
        {
            if (_gameState == null)
            {
                _gameState = ExtUtilityFunction.GetScript<GameState>();
            }
            return;
        }

       if (Input.GetKeyDown(KeyCode.D) && _currentItem != null)
        {
            _currentItem.DropItem(transform.forward);
            _currentItem = null;
        } 
    }
#endif 
}
