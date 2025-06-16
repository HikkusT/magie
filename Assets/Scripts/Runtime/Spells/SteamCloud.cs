using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class SteamCloud : NetworkBehaviour, IPushable
    {
        [SerializeField] private float _timeBetweenHitsInSeconds;
        
        private readonly Dictionary<Player, DateTime> _nextDamageSchedule = new Dictionary<Player, DateTime>();
        public void ReceivePush(Vector3 direction)
        {
            if (!HasAuthority) return;
            
            GetComponent<NetworkObject>().Despawn();
        }

        private void Update()
        {
            if (!IsSpawned || !HasAuthority) return;
            
            DateTime now = DateTime.Now;
            foreach (Player player in _nextDamageSchedule.Where(it => it.Value < now).Select(it => it.Key))
            {
                player.ReceiveDamageServerRpc(1);
                _nextDamageSchedule[player] = now + TimeSpan.FromSeconds(_timeBetweenHitsInSeconds);
            }
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Player")) return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();
            _nextDamageSchedule[collidedPlayer] = DateTime.Now + TimeSpan.FromSeconds(_timeBetweenHitsInSeconds);
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Player")) return;
            
            Player collidedPlayer = collider.GetComponentInParent<Player>();
            _nextDamageSchedule.Remove(collidedPlayer);
        }
    }
}