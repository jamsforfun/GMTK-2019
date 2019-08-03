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

    [FoldoutGroup("Debug"), Tooltip("input for dash"), ReadOnly]
    public bool Action;

    [FoldoutGroup("Object"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private PlayerController _playerController = default;


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
        MoveInput = new Vector2(PlayerConnected.Instance.GetPlayer(_playerController.IdPlayer).GetAxis("Move Horizontal"),
            PlayerConnected.Instance.GetPlayer(_playerController.IdPlayer).GetAxis("Move Vertical"));

        Action = PlayerConnected.Instance.GetPlayer(_playerController.IdPlayer).GetButton("Jump");
    }

    private void Update()
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
