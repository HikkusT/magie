using System;
using System.Linq;
using JetBrains.Annotations;
using Magie.Elements;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Magie.Input
{
    public class CastingFinger : MonoBehaviour
    {
        [SerializeField] private OVRSkeleton _handSkeleton;
        [SerializeField] private OVRSkeleton.BoneId _fingerBoneId;
        [SerializeField] private Element _element;
        
        private Transform _fingertipSlot;
        public event Action<Element> OnCasted;
        
        private bool _visibility = false;
        private ParticleSystem _fingertipParticles;

        private void Start()
        {
            // TODO: Test using handref:
            // handSkeleton = ((OVRHand) _hand).GetComponent<OVRSkeleton>();
            _fingertipSlot = _handSkeleton.Bones.FirstOrDefault(bone => bone.Id == _fingerBoneId)?.Transform;
        }

        public void SetVisibility(bool visibility)
        {
            if (_visibility == visibility) return;

            if (visibility)
            {
                _fingertipParticles = Instantiate(_element.FingertipParticles, _fingertipSlot);
            }
            else
            {
                Destroy(_fingertipParticles.gameObject);
            }

            _visibility = visibility;
        }

        [UsedImplicitly]
        public void TriggerCasted()
        {
            OnCasted?.Invoke(_element);
        }
    }
}