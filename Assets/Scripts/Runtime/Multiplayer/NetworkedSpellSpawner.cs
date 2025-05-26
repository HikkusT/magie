using System.Linq;
using Magie.Input;
using Magie.Spells;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public class NetworkedSpellSpawner : NetworkBehaviour, ISpellSpawner
    {
        [SerializeField] SpellTable _spellTable;
        
        private readonly SinglePlayerSpellSpawner _singlePlayerSpellSpawner = new SinglePlayerSpellSpawner();
        
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
            RequestSpawnServerRpc(prefabId, position, rotation);
        }

        public void SpawnProjectileSpell(Projectile prefab, Vector3 position, Quaternion rotation, Transform target)
        {
            _singlePlayerSpellSpawner.SpawnProjectileSpell(prefab, position, rotation, target);
            
            BroadcastProjectileSpawnServerRpc(prefab.gameObject.name, position, rotation, target.position);
        }

        [ServerRpc]
        private void RequestSpawnServerRpc(uint prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs.First(it => it.SourcePrefabGlobalObjectIdHash == prefabId).Prefab;
            var instance = Instantiate(prefab, position, rotation);
            instance.GetComponent<NetworkObject>().Spawn();
        }
        
        [ServerRpc]
        private void BroadcastProjectileSpawnServerRpc(string spellName, Vector3 position, Quaternion rotation, Vector3 targetPosition, ServerRpcParams serverRpcParams = default)
        {
            SpawnProjectileClientRpc(spellName, position, rotation, targetPosition, serverRpcParams.Receive.SenderClientId);
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc(string spellName, Vector3 position, Quaternion rotation, Vector3 targetPosition, ulong owner)
        {
            if (NetworkManager.Singleton.LocalClientId == owner) return;
    
            ProjectileSpell spell = _spellTable.LookUp.Values.Cast<ProjectileSpell>().First(it => it?.Prefab.name == spellName);
            if (spell == null) return;
            
            Projectile projectile = Instantiate(spell.Prefab, position, rotation);
            projectile.PlayTrajectory(targetPosition, owner);
        }

    }
}