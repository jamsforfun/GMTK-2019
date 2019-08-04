using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerLinker : MonoBehaviour
{
    [Serializable]
    public struct ReferenceButton
    {
        public string TextButton;
        public PlayerLinker PlayerLinker;
    }

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public PlayerLinker[] PlayerLinker;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public Transform AllItems;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public ReferenceButton[] ReferenceButtonPlayer;

    public void Init()
    {
        for (int i = 0; i < PlayerLinker.Length; i++)
        {
            PlayerLinker[i].Init(this);
        }
    }

    /// <summary>
    /// get an action from someon , with an index
    /// index correspond to the id of the player (not id if the array !)
    /// 
    /// return the player found with the id
    /// </summary>
    /// <param name="fromAction"></param>
    /// <returns></returns>
    public PlayerLinker ActionButton(PlayerLinker fromAction, int id)
    {
        for (int i = 0; i < ReferenceButtonPlayer.Length; i++)
        {
            if (ReferenceButtonPlayer[i].PlayerLinker.PlayerManager.Id == id)
            {
                return (ReferenceButtonPlayer[i].PlayerLinker);
            }
        }
        return (null);
    }

    public bool IsThatColliderIsAPlayer(Collider isPlayer, out PlayerLinker playerLinker)
    {
        playerLinker = null;

        if (isPlayer == null)
        {
            return (false);
        }

        for (int i = 0; i < PlayerLinker.Length; i++)
        {
            if (PlayerLinker[i].ColliderPlayer.GetInstanceID() == isPlayer.GetInstanceID())
            {
                playerLinker = PlayerLinker[i];
                return (true);
            }
        }
        return (false);
    }
}
