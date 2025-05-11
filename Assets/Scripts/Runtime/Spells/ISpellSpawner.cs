using UnityEngine;

namespace Magie.Spells
{
    public interface ISpellSpawner
    {
        void SpawnConstructionSpell(GameObject prefab, Vector3 position, Quaternion rotation);
    }
}