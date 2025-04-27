using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magie.Spells
{
    public class ConstructionSpellFiringContext : ASpellFiringContext
    {
        private readonly ConstructionSpell Spell;
        
        public ConstructionSpellFiringContext(ConstructionSpell spell, Action onDepleted) : base(onDepleted)
        {
            Spell = spell;
        }

        public override void TryFire(Vector3 spellOriginPosition, Transform target)
        {
            Vector3 desiredForward = Vector3.ProjectOnPlane(target.position - spellOriginPosition, target.up).normalized;
            Object.Instantiate(Spell.Prefab, target.position, Quaternion.LookRotation(desiredForward,target.up));
            OnDepleted?.Invoke();
        }
    }
}