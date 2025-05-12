using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Construction : NetworkBehaviour
    {
        [SerializeField] private float _ttlInSeconds;
        [SerializeField] private float _scaleDurationInSeconds = 0.5f;
        [SerializeField] private Ease _scaleEase = Ease.InOutCubic;
        [SerializeField] private List<ParticleSystem> _scaleParticles;
        [SerializeField] private Transform _visualRoot;

        public override void OnNetworkSpawn()
        {
            _scaleParticles.ForEach(it => it.Play());
            _visualRoot.DOScaleY(1, _scaleDurationInSeconds).SetEase(_scaleEase)
                .OnComplete(() => _scaleParticles.ForEach(it => it.Stop()));

            if (IsOwner)
            {
                DespawnAfter(TimeSpan.FromSeconds(_ttlInSeconds)).Forget();
            }
        }

        private async UniTaskVoid DespawnAfter(TimeSpan duration)
        {
            await UniTask.Delay(duration);
            
            GetComponent<NetworkObject>().Despawn();
        }
    }
}