using System;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Beam Spell")]
    public class BeamSpell: Spell
    {
        [SerializeField] private GameObject _prefab;
        
        public override ASpellFiringContext CreateContext(Transform spellOrigin, Action onContextClosure) =>
            new BeamSpellFiringContext(this, spellOrigin, onContextClosure);
    }
}