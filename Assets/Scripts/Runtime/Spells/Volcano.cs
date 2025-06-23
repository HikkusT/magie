using System;
using System.Collections.Generic;
using DG.Tweening;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Volcano : NetworkBehaviour
    {
        private readonly static int GLOW_POWER_PROP_ID = Shader.PropertyToID("_GlowPower");
        
        [SerializeField] private int _damage;
        [SerializeField] private ParticleSystem _triggerVFX;
        [SerializeField] private Construction _construction;
        
        [Header("Scale Config")]
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private Vector3 _scale;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private Ease _scaleEase;
        
        [Header("Fissure Config")]
        [SerializeField] private List<Renderer> _fissureRenderers;
        [SerializeField] private Ease _fissurePowerEase;
        [SerializeField] private float _finalPower = 0.5f;
        
        private readonly HashSet<Player> _playersInArea = new HashSet<Player>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            _visualRoot.transform.localScale = Vector3.zero;
            _visualRoot.transform.DOScale(_scale, _scaleDuration).SetEase(_scaleEase);
            
            foreach (var fissureRenderer in _fissureRenderers)
            {
                fissureRenderer.material.DOFloat(_finalPower, GLOW_POWER_PROP_ID, (float) _construction.TTL.TotalSeconds).SetEase(_fissurePowerEase);
            }
        }
        
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