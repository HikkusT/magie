using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Magie
{
    public class MatchmakerManager : MonoBehaviour
    {
        private const string RELAY_CODE_PARAMETER_KEY = "relay_code";
        
        [SerializeField] private int _maxPlayers = 2;

        private void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        { 
            await UnityServices.InitializeAsync();
            
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in as: {AuthenticationService.Instance.PlayerId}");
            }

            await TryJoinOrCreateLobby(this.GetCancellationTokenOnDestroy());
        }

        private async UniTask TryJoinOrCreateLobby(CancellationToken ct)
        {
            Lobby joinedLobby;
 
            try
            {
                joinedLobby = await JoinRandomLobby();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Join lobby failed, creating one...");
                
                joinedLobby = await CreateLobby();
            }

            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(15), cancellationToken: ct);

                if (joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
                {
                    await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        private async UniTask<Lobby> JoinRandomLobby()
        {
            Lobby foundLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            string relayJoinCode = foundLobby.Data[RELAY_CODE_PARAMETER_KEY].Value;

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key,
                allocation.ConnectionData, allocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();

            return foundLobby;
        }

        private async UniTask<Lobby> CreateLobby()
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(_maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key,
                allocation.ConnectionData);

            CreateLobbyOptions lobbyConfig = new()
            {
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>
                {
                    { RELAY_CODE_PARAMETER_KEY, new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
                }
            };

            var createdLobby = await LobbyService.Instance.CreateLobbyAsync($"Auto {DateTime.Now}", _maxPlayers, lobbyConfig);
            NetworkManager.Singleton.StartHost();

            return createdLobby;
        }
    }
}