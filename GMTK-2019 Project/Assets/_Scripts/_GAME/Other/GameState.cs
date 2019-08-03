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
        OVER = 30,
    }

    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public StateGame StateOfGame = StateGame.NONE;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private AllPlayerLinker _allPlayerLinker;

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
}
