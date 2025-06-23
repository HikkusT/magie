using System;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class Projectile : MonoBehaviour, IPushable
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _onCollisionVfx;

        private ulong _ownerId;

        private DateTime _gracePeriodUntil;

        public void PlayTrajectory(Vector3 target, ulong ownerId)
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.linearVelocity = direction * _speed;
            _ownerId = ownerId;
            _gracePeriodUntil = DateTime.Now.AddSeconds(0.2f);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.name == "LocalPlayer") return;
            if ((LayerMask.GetMask("IgnoreSpellCollision") & ( 1 << collider.gameObject.layer)) != 0) return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();

            if (collidedPlayer != null && (_ownerId == collidedPlayer.OwnerClientId && DateTime.Now < _gracePeriodUntil))
            {
                return;
            }

            if (_ownerId == NetworkManager.Singleton.LocalClientId)
            {
                if (collidedPlayer != null)
                {
                    PlayerCollision(collidedPlayer);
                }
                else
                {
                    TerrainCollision();
                }
            }
            
            Instantiate(_onCollisionVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        protected virtual void PlayerCollision(Player player)
        {
        }

        protected virtual void TerrainCollision()
        {
            
        }

        public void ReceivePush(Vector3 direction)
        {
            // _rigidbody.linearVelocity = Vector3.Slerp(_rigidbody.linearVelocity, direction * _speed, 0.1f);
            _rigidbody.linearVelocity = direction * _speed;
        }
    }
}