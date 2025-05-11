using System.Linq;
using Magie.Input;
using Magie.Spells;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public class NetworkedSpellSpawner : NetworkBehaviour, ISpellSpawner
    {
        private void Start()
        {
            if (IsOwner)
            {
                FindFirstObjectByType<SpellFiringController>().SetupWithNetwork(this);
            }
        }

        public void SpawnConstructionSpell(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            uint prefabId = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs.First(it => it.Prefab == prefab).SourcePrefabGlobalObjectIdHash;
            RequestSpawnServerRpc(prefabId, position, rotation);;
        }
        
        [ServerRpc]
        private void RequestSpawnServerRpc(uint prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs.First(it => it.SourcePrefabGlobalObjectIdHash == prefabId).Prefab;
            var instance = Instantiate(prefab, position, rotation);
            instance.GetComponent<NetworkObject>().Spawn();
        }
    }
}