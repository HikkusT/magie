using System;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Beam Spell")]
    public class BeamSpell: Spell
    {
        [SerializeField] private Beam _prefab;
        [SerializeField] private float _maxBeamDurationInSeconds;

        public Beam Prefab => _prefab;
        public TimeSpan MaxBeamDuration => TimeSpan.FromSeconds(_maxBeamDurationInSeconds);
        
        public override ASpellFiringContext CreateContext(Transform spellOrigin, Action onContextClosure) =>
            new BeamSpellFiringContext(this, spellOrigin, onContextClosure);
    }
}