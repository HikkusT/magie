using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;
using Multiplayer;
using TMPro;

namespace Magie.Match
{
    public class MatchManager : NetworkBehaviour
    {
        [SerializeField] private List<PlayerReadyZone> _readyZones;
        [SerializeField] private MatchCountdown _countdownPrefab;
        [SerializeField] private TMP_Text _debugText;

        private readonly NetworkVariable<MatchState> _matchState = new ();
        private readonly List<Player> _playersInMatch = new ();
        private MatchCountdown _countdown;

        public override void OnNetworkSpawn()
        {
            _matchState.OnValueChanged += (oldState, newState) =>
            {
                switch (newState)
                {
                    case MatchState.WaitingForPlayers:
                    {
                        _readyZones.ForEach(it => it.gameObject.SetActive(true));
                        break;
                    }
                    case MatchState.Countdown:
                    {
                        _readyZones.ForEach(it => it.Freeze());
                        break;
                    }
                    case MatchState.Running:
                    {
                        _readyZones.ForEach(it => it.gameObject.SetActive(false));
                        break;
                    }
                }
            };
        }
        
        private void Update()
        {
            UpdateDebugText();
            
            if (!IsServer) return;

            switch (_matchState.Value)
            {
                case MatchState.WaitingForPlayers:
                { 
                    if (_readyZones.Any(it => !it.IsReady)) break;
                    
                    _matchState.Value = MatchState.Countdown;
                    _playersInMatch.AddRange(FindObjectsOfType<Player>());
                    _countdown = Instantiate(_countdownPrefab, Vector3.zero, Quaternion.identity);
                    _countdown.GetComponent<NetworkObject>().Spawn();
                    break;
                }
                case MatchState.Countdown:
                {
                    if (!_countdown.Finished) break;
                    
                    _matchState.Value = MatchState.Running;
                    _countdown.GetComponent<NetworkObject>().Despawn();
                    break;
                }
                case MatchState.Running:
                {
                    if (_playersInMatch.All(it => it != null) && _playersInMatch.All(it => it.CurrentHealth.Value > 0)) break;
                    
                    _matchState.Value = MatchState.WaitingForPlayers;
                    break;
                }
            } 
        }

        private void UpdateDebugText()
        {
            if (_debugText == null) return;
            
            _debugText.text = 
                $"Match State: {_matchState.Value}\n" +
                $"Ready players count: {_readyZones.Count(it => it.IsReady)}\n";
        }
        
        private enum MatchState
        {
            WaitingForPlayers,
            Countdown,
            Running
        }
    }
} 