using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum StateGame
    {
        NONE = 0,
        LOADING = 10,
        GAME = 20,
        DOOR_OPEN = 30,
    }

    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public StateGame StateOfGame = StateGame.NONE;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private AllPlayerLinker _allPlayerLinker;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private BinaryTimer _binaryTimer;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Door _door;

    public bool CanMovePlayer()
    {
        if (StateOfGame == StateGame.NONE || StateOfGame == StateGame.LOADING)
        {
            return (false);
        }
        return (true);
    }

    public void Awake()
    {
        StateOfGame = StateGame.GAME;
        StartGame();
    }

    public void StartGame()
    {
        _allPlayerLinker.Init();
    }

    private void TestIfGameEnded()
    {
        if (StateOfGame == StateGame.GAME && _binaryTimer.TimerEnded)
        {
            StateOfGame = StateGame.DOOR_OPEN;
            _door.OpenDoor();
        }
    }

    private void Update()
    {
        TestIfGameEnded();
    }
}
