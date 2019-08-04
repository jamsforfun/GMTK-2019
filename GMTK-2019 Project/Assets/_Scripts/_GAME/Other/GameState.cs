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
        PAUSE = 40,
        WIN_GAME = 50,
    }

    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public StateGame StateOfGame = StateGame.NONE;
    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public float _timeDoorOpen = 20f;


    [FoldoutGroup("GamePlay"), Tooltip("ref")]
    public GameObject EndRoom;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private GameUI _gameUi;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private AllPlayerLinker _allPlayerLinker;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private BinaryTimer _binaryTimer;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private SceneLoader _sceneLoader;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private AllCameras _allCameras;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Door _door;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private PlayerConnected _playerConnected;

    private FrequencyCoolDown _timeDoorClose = new FrequencyCoolDown();


    private StateGame _lastState;

    public bool CanMovePlayer()
    {
        if (StateOfGame == StateGame.NONE
            || StateOfGame == StateGame.LOADING
            || StateOfGame == StateGame.PAUSE
            /*|| StateOfGame == StateGame.WIN_GAME*/)
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
        if (EndRoom)
        {
            EndRoom.SetActive(false);
        }

        List<Transform> allCams = new List<Transform>();
        for (int i = 0; i < _playerConnected.playerArrayConnected.Length; i++)
        {
            bool activeThisOne = true;// (i < 2) ? true : _playerConnected.playerArrayConnected[i];
            _allPlayerLinker.PlayerLinker[i].gameObject.SetActive(activeThisOne);
            if (activeThisOne)
            {
                allCams.Add(_allPlayerLinker.PlayerLinker[i].Rigidbody.transform);
            }
        }

        _allCameras.ActiveMainCamera(allCams);
    }

    private void TestIfGameEnded()
    {
        if (StateOfGame == StateGame.GAME && _binaryTimer.TimerEnded)
        {
            StateOfGame = StateGame.DOOR_OPEN;
            _door.OpenDoor();
            EndRoom.SetActive(true);
            _allCameras.DoorOpen();
            _timeDoorClose.StartCoolDown(_timeDoorOpen);
        }
        if (StateOfGame == StateGame.DOOR_OPEN && _timeDoorClose.IsStartedAndOver())
        {
            WinGame();
        }
    }

    public bool Pause()
    {
        if (StateOfGame == StateGame.PAUSE)
        {
            return (false);
        }
        _lastState = StateOfGame;

        StateOfGame = StateGame.PAUSE;
        _gameUi.Pause();

        Time.timeScale = 0;

        return (true);
    }

    public void UnPause()
    {
        StateOfGame = _lastState;
        _gameUi.UnPause();

        Time.timeScale = 1;
    }

    private void PauseInput()
    {
        if (PlayerConnected.Instance.GetButtonUpFromAnyGamePad("Start"))
        {
            if (StateOfGame == StateGame.PAUSE)
            {
                UnPause();
            }
            else if (StateOfGame == StateGame.GAME || StateOfGame == StateGame.DOOR_OPEN)
            {
                Pause();
            }
        }
        if (StateOfGame == StateGame.PAUSE)
        {
            _gameUi.CustomSelectPause();
            if (PlayerConnected.Instance.GetButtonUpFromAnyGamePad("FireB"))
            {
                UnPause();
            }
        }
    }

    private void WinInputs()
    {
        if (StateOfGame == StateGame.WIN_GAME)
        {
            _gameUi.CustomSelectWin();
        }
    }

    public void WinGame()
    {
        UnPause();
        StateOfGame = StateGame.WIN_GAME;
        _allCameras.ActiveWinCam();
        _gameUi.Win();
        Debug.Log("win panel active");
    }

    public void GoBackToMenu()
    {
        UnPause();
        _sceneLoader.LoadSceneByTrackIndex(0);
    }

    private void Update()
    {
        PauseInput();
        WinInputs();

        

        TestIfGameEnded();

    }
}
