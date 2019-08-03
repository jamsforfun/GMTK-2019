using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// InputPlayer Description
/// </summary>
[TypeInfoBox("Player input")]
public class PlayerInput : MonoBehaviour
{
    [FoldoutGroup("Debug"), Tooltip("Move input"), ReadOnly]
    public Vector2 MoveInput;
    [FoldoutGroup("Debug"), Tooltip("Move input"), ReadOnly]
    public Vector2 MoveJoystick2;

    [FoldoutGroup("Debug"), Tooltip(""), ReadOnly]
    public bool FireA;
    [FoldoutGroup("Debug"), Tooltip(""), ReadOnly]
    public bool FireB;
    [FoldoutGroup("Debug"), Tooltip("h"), ReadOnly]
    public bool FireY;
    [FoldoutGroup("Debug"), Tooltip("h"), ReadOnly]
    public bool FireX;

    [FoldoutGroup("Debug"), Tooltip(""), ReadOnly]
    public bool PressJoystickButton;

    [FoldoutGroup("Debug"), Tooltip(""), ReadOnly]
    public bool Start;
    [FoldoutGroup("Debug"), Tooltip(""), ReadOnly]
    public bool Select;


    [FoldoutGroup("Object"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private PlayerManager _playerManager = default;


    private void OnEnable()
    {
        EventManager.StartListening(GameData.Event.GameOver, GameOver);
    }

    /// <summary>
    /// tout les input du jeu, à chaque update
    /// </summary>
    private void GetInput()
    {
        //all axis
        MoveInput = new Vector2(PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetAxis("Move Horizontal"),
            PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetAxis("Move Vertical"));

        MoveJoystick2 = new Vector2(PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetAxis("Move Horizontal Right"),
            PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetAxis("Move Vertical Right"));

        FireA = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("FireA");
        FireB = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("FireB");
        FireY = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("FireY");
        FireX = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("FireX");

        PressJoystickButton = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("PressJoystickButton");

        Start = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("Start");
        Select = PlayerConnected.Instance.GetPlayer(_playerManager.Id).GetButtonUp("Select");
    }

    public bool IsMoving(float margin = 0.1f)
    {
        return (MoveInput.sqrMagnitude > margin);
    }

    public Vector2 GetMoveDirection(bool normalize = true)
    {
        if (normalize)
        {
            return (MoveInput.FastNormalized());
        }
        return (MoveInput);
    }

    /// <summary>
    /// get the maghitude of the input, set sqrMagnitude at true if you want more optimisation
    /// </summary>
    /// <param name="sqrMagnitude"></param>
    /// <returns></returns>
    public float GetMagnitudeInput(bool sqrMagnitude = false)
    {
        if (sqrMagnitude)
        {
            return (MoveInput.sqrMagnitude);
        }
        return (MoveInput.magnitude);
    }

    /// <summary>
    /// called each update
    /// </summary>
    public void CustomUpdate()
    {
        GetInput();
    }

    private void GameOver()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.StartListening(GameData.Event.GameOver, GameOver);
    }
}
