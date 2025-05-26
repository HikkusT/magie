using DG.Tweening;
using UnityEngine;

namespace Magie.Spells
{
    public class TeleportHitVfx : MonoBehaviour
    {
        [SerializeField] private float _durationInSeconds = 0.5f;
        [SerializeField] private float _maxScale = 1f;

        private void Start()
        {
            Sequence scaleSequence = DOTween.Sequence();

            scaleSequence.Append(transform.DOScale(_maxScale, _durationInSeconds * 0.5f)
                .SetEase(Ease.OutBack));

            scaleSequence.Append(transform.DOScale(0f, _durationInSeconds * 0.5f)
                .SetEase(Ease.InBack));
            
            scaleSequence.OnComplete(() => Destroy(gameObject));
        }
    }
}