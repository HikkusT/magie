using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class PlayerSpaw : MonoBehaviour
    {
        [SerializeField] private Vector3 _spawnCenter;
        [SerializeField] private float _spawnRadius;

        private void Start()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
            transform.position = _spawnCenter + new Vector3(randomOffset.x, 0, randomOffset.y);
            transform.LookAt(_spawnCenter);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_spawnCenter, _spawnRadius);
        }
    }
}