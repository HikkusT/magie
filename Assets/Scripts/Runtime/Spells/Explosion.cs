using System.Collections.Generic;
using DG.Tweening;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Explosion : NetworkBehaviour
    {
        [SerializeField] private Construction _construction;
        [SerializeField] private int _damage;
        [SerializeField] private float _finalScale;
        [SerializeField] private Ease _scaleEase;
        
        private readonly HashSet<Player> _damagedPlayers = new HashSet<Player>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!HasAuthority) return;
            
            transform.DOScale(_finalScale, (float) _construction.TTL.TotalSeconds).SetEase(_scaleEase);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!HasAuthority || !IsSpawned) return;
            
            Player collidedPlayer = other.GetComponentInParent<Player>();
            if (collidedPlayer != null && collidedPlayer.OwnerClientId != _construction.CasterId && !_damagedPlayers.Contains(collidedPlayer))
            {
                collidedPlayer.ReceiveDamageServerRpc(_damage);
                _damagedPlayers.Add(collidedPlayer);
            }
        }
    }
}