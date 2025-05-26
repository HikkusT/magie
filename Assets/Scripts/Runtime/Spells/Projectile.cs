using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _onCollisionVfx;

        private ulong _ownerId;

        public void PlayTrajectory(Vector3 target, ulong ownerId)
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.linearVelocity = direction * _speed;
            _ownerId = ownerId;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.name == "LocalPlayer") return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();

            if (collidedPlayer != null && _ownerId == collidedPlayer.OwnerClientId)
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
    }
}