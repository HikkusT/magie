using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Magie.Match
{
    public class MatchCountdown : NetworkBehaviour
    {
        [SerializeField] private int _countdownSeconds;
        [SerializeField] private TMP_Text _countdownText;
        
        private readonly NetworkVariable<int> _countdown = new NetworkVariable<int>(int.MaxValue);
        private DateTime _countdownFinishesAt;
        
        public bool Finished => _countdown.Value <= 0;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _countdown.Value = _countdownSeconds;
                _countdownFinishesAt = DateTime.Now + TimeSpan.FromSeconds(_countdownSeconds);
            }
        }
        
        private void Update()
        {
            _countdownText.enabled = _countdown.Value >= 0 && _countdown.Value <= _countdownSeconds;
            _countdownText.text = _countdown.Value.ToString();
            _countdownText.transform.parent.forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
            
            
            if (IsServer)
            {
                _countdown.Value = Mathf.CeilToInt((float) (_countdownFinishesAt - DateTime.Now).TotalSeconds);
            }
        }
    }
}