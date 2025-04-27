using System;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Construction Spell")]
    public class ConstructionSpell: Spell
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;
        
        public override ASpellFiringContext CreateContext(Action onContextClosure) =>
            new ConstructionSpellFiringContext(this, onContextClosure);
    }
}