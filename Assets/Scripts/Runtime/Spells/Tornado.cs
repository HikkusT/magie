using System;
using DG.Tweening;
using Multiplayer;
using Oculus.Interaction;
using UnityEngine;
using Tween = Oculus.Interaction.Tween;

namespace Magie.Spells
{
    public class Tornado : MonoBehaviour, IPushable
    {
        [SerializeField] private float _lift;
        [SerializeField] private float _power = 7f;
        [SerializeField] private float _pushFactor;

        OVRPlayerController _localPlayer = null;
        private static DG.Tweening.Tween _vignetteAnimation = null;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("LocalPlayer")) return;

            if (_localPlayer == null)
            {
                _localPlayer = FindObjectOfType<OVRPlayerController>();
                if (_vignetteAnimation.IsActive())
                {
                    _vignetteAnimation.Kill();
                }
                TunnelingEffect tunnelingEffect = FindObjectOfType<TunnelingEffect>();
                _vignetteAnimation = DOTween.To(() => tunnelingEffect.AlphaStrength, x => tunnelingEffect.AlphaStrength = x, 0.9f, .3f);
            }
            
            _localPlayer.GravityModifier = 0f;
            // _localPlayer.transform.position = Vector3.Lerp(_localPlayer.transform.position, new Vector3(_localPlayer.transform.position.x, _lift, _localPlayer.transform.position.z), 0.1f);
            _localPlayer.transform.position += _power * Time.fixedDeltaTime * (_lift - _localPlayer.transform.position.y) * Vector3.up;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("LocalPlayer")) return;
            
            OnDisable();
        }
        
        public void ReceivePush(Vector3 direction)
        {
            // if (!HasAuthority) return;
            
            transform.position += Vector3.ProjectOnPlane(direction, transform.up) * _pushFactor * Time.fixedDeltaTime;
        }

        private void OnDisable()
        {
            if (_localPlayer == null) return;
            
            _localPlayer.GravityModifier = 0.1f;
            _localPlayer = null;

            if (_vignetteAnimation.IsActive())
            {
                _vignetteAnimation.Kill();
            }
            TunnelingEffect tunnelingEffect = FindObjectOfType<TunnelingEffect>();
            _vignetteAnimation = DOTween.To(() => tunnelingEffect.AlphaStrength, x => tunnelingEffect.AlphaStrength = x, 0f, 1.5f).SetEase(Ease.InCubic);
        }
    }
}