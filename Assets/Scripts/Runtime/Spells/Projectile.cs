using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Projectile : MonoBehaviour
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

            if (collidedPlayer != null && _ownerId == NetworkManager.Singleton.LocalClientId)
            {
                collidedPlayer.ReceiveDamageServerRpc(1);
            }
            
            Instantiate(_onCollisionVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}