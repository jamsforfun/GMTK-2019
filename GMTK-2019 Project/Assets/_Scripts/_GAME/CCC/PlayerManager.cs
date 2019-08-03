using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Main player controller")]
public class PlayerManager : MonoBehaviour, IKillable
{
    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public PlayerSettings PlayerSettings;

    [FoldoutGroup("GamePlay"), SerializeField, Tooltip("death vibration")]
    private Vibration _deathVibration = new Vibration();

    [FoldoutGroup("Object"), Tooltip("ref script")]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("Object"), Tooltip("ref script")]
    private PlayerInput _playerInput;

    public int Id { get { return (PlayerSettings.Id); } set { PlayerSettings.Id = value; } }

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
        PlayerConnected.Instance.SetVibrationPlayer(Id, _deathVibration);
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
