using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Collider))]
public class Landfill : MonoBehaviour
{
    [SerializeField, ReadOnly] private PlayerLinker[] _allPlayerLinkers;
    [SerializeField] private float _searchCooldown = 2f;
    private List<FrequencyCoolDown> _playerCooldowns;

#if UNITY_EDITOR
    private void Update()
    {
        if (_allPlayerLinkers == null)
        {
            _allPlayerLinkers = FindObjectOfType<AllPlayerLinker>().PlayerLinker;
            for (int i = 0; i < _allPlayerLinkers.Length; i++)
            {
                _playerCooldowns.Add(new FrequencyCoolDown());
            }
        }
    }
#endif

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerLinker collidingPlayerLinker = default;
        foreach(PlayerLinker playerLinker in _allPlayerLinkers)
        {
            if (other == playerLinker.ColliderPlayer)
            {
                collidingPlayerLinker = playerLinker;
            }
        }
        if (collidingPlayerLinker != null && !collidingPlayerLinker.PlayerObjectInteraction.HasItem)
        {
            
        }
    }

    private IEnumerator SearchCoroutine(PlayerLinker playerLinker)
    {
        float timeSinceStart = 0f;
        while (timeSinceStart < _searchCooldown)
        {
            yield return null;
            timeSinceStart += Time.deltaTime;
        }
        // give a random object to the player
    }
}
