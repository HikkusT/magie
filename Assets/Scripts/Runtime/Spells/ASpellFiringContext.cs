using System;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class ASpellFiringContext
    {
        protected readonly Transform SpellOrigin;
        protected readonly Action OnDepleted;
        
        protected ASpellFiringContext(Transform spellOrigin, Action onDepleted)
        {
            SpellOrigin = spellOrigin;
            OnDepleted = onDepleted;
        }
        
        public abstract void TryFire(Transform target);

        public abstract void TryCancel();
    }
}