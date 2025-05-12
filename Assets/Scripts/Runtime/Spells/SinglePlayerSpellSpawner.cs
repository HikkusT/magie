using UnityEngine;

namespace Magie.Spells
{
    public class SinglePlayerSpellSpawner : ISpellSpawner
    {
        public void SpawnConstructionSpell(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            Object.Instantiate(prefab, position, rotation);
        }
    }
}