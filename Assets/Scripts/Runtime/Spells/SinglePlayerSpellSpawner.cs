using Unity.Netcode;
using UnityEngine;

namespace Magie.Spells
{
    public class SinglePlayerSpellSpawner : ISpellSpawner
    {
        public void SpawnConstructionSpell(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            Object.Instantiate(prefab, position, rotation);
        }

        public void SpawnProjectileSpell(Projectile prefab, Vector3 position, Quaternion rotation, Transform target)
        {
            Projectile projectile = Object.Instantiate(prefab, position, rotation);
            projectile.PlayTrajectory(target.position, NetworkManager.Singleton.LocalClientId);
        }
    }
}