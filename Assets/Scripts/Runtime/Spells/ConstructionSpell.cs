using System;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Construction Spell")]
    public class ConstructionSpell: Spell
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _alwaysWorldUp;
        [SerializeField] private bool _fromPlayer;

        public GameObject Prefab => _prefab;
        public bool AlwaysWorldUp => _alwaysWorldUp;
        public bool FromPlayer => _fromPlayer;
        
        public override ASpellFiringContext CreateContext(Transform spellOrigin, Action onContextClosure) =>
            new ConstructionSpellFiringContext(this, spellOrigin, onContextClosure);
    }
}