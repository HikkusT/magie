using System.Collections.Generic;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Volcano : NetworkBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private ParticleSystem _triggerVFX;
        
        private readonly HashSet<Player> _playersInArea = new HashSet<Player>();

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Player")) return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();
            _playersInArea.Add(collidedPlayer);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Player")) return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();
            _playersInArea.Remove(collidedPlayer);
        }

        public override void OnNetworkDespawn()
        {
            Instantiate(_triggerVFX, transform.position, Quaternion.identity);

            if (HasAuthority)
            {
                foreach (Player player in _playersInArea)
                {
                    player.ReceiveDamageServerRpc(_damage);
                }
            }

            base.OnNetworkDespawn();
        }
    }
}