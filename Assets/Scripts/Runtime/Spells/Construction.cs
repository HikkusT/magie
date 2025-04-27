using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Magie.Spells
{
    public class Construction : MonoBehaviour
    {
        [SerializeField] private float _ttlInSeconds;
        [SerializeField] private float _scaleDurationInSeconds = 0.5f;
        [SerializeField] private Ease _scaleEase = Ease.InOutCubic;
        [SerializeField] private List<ParticleSystem> _scaleParticles;
        [SerializeField] private Transform _visualRoot;

        private void Start()
        {
            _scaleParticles.ForEach(it => it.Play());
            _visualRoot.DOScaleY(1, _scaleDurationInSeconds).SetEase(_scaleEase)
                .OnComplete(() => _scaleParticles.ForEach(it => it.Stop()));
            Destroy(gameObject, _ttlInSeconds);
        }
    }
}