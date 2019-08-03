using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Main player controller")]
public class PlayerController : MonoBehaviour, IKillable
{
    [FoldoutGroup("GamePlay"), SerializeField, Tooltip("id player for input")]
    public int IdPlayer = 0;

    [FoldoutGroup("GamePlay"), SerializeField, Tooltip("death vibration")]
    private Vibration _deathVibration = new Vibration();

    [FoldoutGroup("Object"), Tooltip("ref script")]
    private PlayerInput _playerInput;

    [FoldoutGroup("Object"), Tooltip("ref script")]
    public Transform renderPlayer = null;

    


    private bool isMoving = false;


    private void OnEnable()
    {
        EventManager.StartListening(GameData.Event.GameOver, GameOver);
    }

    private void Awake()
    {

    }

    /// <summary>
    /// called when the game is over: desactive player
    /// </summary>
    public void GameOver()
    {
        PlayerConnected.Instance.SetVibrationPlayer(IdPlayer, _deathVibration);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameData.Event.GameOver, GameOver);
    }

    public void Kill()
    {
        
    }

    public void GetHit(int amount, Vector3 posAttacker)
    {
        //throw new System.NotImplementedException();
    }
}
