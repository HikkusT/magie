using System;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class ASpellFiringContext
    {
        protected readonly Transform SpellOrigin;
        protected readonly Action OnDepleted;

        public abstract Spell SpellInUse { get; } 
        
        protected ASpellFiringContext(Transform spellOrigin, Action onDepleted)
        {
            SpellOrigin = spellOrigin;
            OnDepleted = onDepleted;
        }
        
        public abstract void TryFire(Transform target, ISpellSpawner spellSpawner);

        public abstract void TryCancel();
    }
}