using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magie.Spells
{
    public class ConstructionSpellFiringContext : ASpellFiringContext
    {
        private readonly ConstructionSpell Spell;
        
        public ConstructionSpellFiringContext(ConstructionSpell spell, Transform spellOrigin, Action onDepleted) : base(spellOrigin, onDepleted)
        {
            Spell = spell;
        }

        public override void TryFire(Transform target, ISpellSpawner spellSpawner)
        {
            Vector3 desiredForward = Vector3.ProjectOnPlane(target.position - SpellOrigin.transform.position, Spell.AlwaysWorldUp ? Vector3.up : target.up).normalized;
            spellSpawner.SpawnConstructionSpell(Spell.Prefab,
                Spell.CalculateSpawnPosition(SpellOrigin.transform, target),
                Quaternion.LookRotation(desiredForward, Spell.AlwaysWorldUp ? Vector3.up : target.up));
            OnDepleted?.Invoke();
        }

        public override void TryCancel()
        {
        }
    }
}