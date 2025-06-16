using System;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Magie.Spells
{
    public class Boulder : NetworkBehaviour
    {
        [SerializeField] private CollisionEventRaiser _collisionListener;
        [SerializeField] private Rigidbody _boulder;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _angularSpeed;

        private void Start()
        {
            _collisionListener.OnTriggerEntered += HandleCollision;
        }

        private void FixedUpdate()
        {
            if (!IsSpawned || !HasAuthority) return;
            
            transform.position += transform.forward * _speed * Time.fixedDeltaTime;
            _boulder.transform.Rotate(Vector3.right, _angularSpeed * Time.fixedDeltaTime, Space.Self);
        }

        private void HandleCollision(Collider other)
        {
            if (!IsSpawned || !HasAuthority) return;
            if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Solid")) return;
            
            Player collidedPlayer = other.GetComponentInParent<Player>();
            if (collidedPlayer != null)
            {
                collidedPlayer.ReceiveDamageServerRpc(_damage);
            }
            
            Construction construction = other.GetComponentInParent<Construction>();
            if (construction != null)
            {
                construction.GetComponent<NetworkObject>().Despawn(); // TODO: Specific wall
            }
            
            GetComponent<NetworkObject>().Despawn();
        }
    }
}