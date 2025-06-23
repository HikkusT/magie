using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SaintsField;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class Construction : NetworkBehaviour
    {
        [SerializeField] private float _ttlInSeconds;
        [SerializeField] private bool _shouldApplyeffects = true;
        [SerializeField, ShowIf(nameof(_shouldApplyeffects))] private float _scaleDurationInSeconds = 0.5f;
        [SerializeField, ShowIf(nameof(_shouldApplyeffects))] private Ease _scaleEase = Ease.InOutCubic;
        [SerializeField, ShowIf(nameof(_shouldApplyeffects))] private List<ParticleSystem> _scaleParticles;
        [SerializeField, ShowIf(nameof(_shouldApplyeffects))] private Transform _visualRoot;
        
        public TimeSpan TTL => TimeSpan.FromSeconds(_ttlInSeconds);

        public override void OnNetworkSpawn()
        {
            if (_shouldApplyeffects)
            {
                _scaleParticles.ForEach(it => it.Play());
                _visualRoot.DOScaleY(1, _scaleDurationInSeconds).SetEase(_scaleEase)
                    .OnComplete(() => _scaleParticles.ForEach(it => it.Stop()));
            }

            if (IsOwner)
            {
                DespawnAfter(TimeSpan.FromSeconds(_ttlInSeconds), this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        private async UniTaskVoid DespawnAfter(TimeSpan duration, CancellationToken ct)
        {
            await UniTask.Delay(duration, cancellationToken: ct);
            
            GetComponent<NetworkObject>().Despawn();
        }
    }
}