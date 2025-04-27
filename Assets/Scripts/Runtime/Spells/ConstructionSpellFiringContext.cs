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

        public override void TryFire(Transform target)
        {
            Vector3 desiredForward = Vector3.ProjectOnPlane(target.position - SpellOrigin.transform.position, target.up).normalized;
            Object.Instantiate(Spell.Prefab, target.position, Quaternion.LookRotation(desiredForward,target.up));
            OnDepleted?.Invoke();
        }

        public override void TryCancel()
        {
        }
    }
}