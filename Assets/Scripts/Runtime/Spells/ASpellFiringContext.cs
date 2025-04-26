using System;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class ASpellFiringContext
    {
        protected readonly Action OnDepleted;
        
        protected ASpellFiringContext(Action onDepleted)
        {
            OnDepleted = onDepleted;
        }
        
        public abstract void TryTriggerSpell(Vector3 spellOriginPosition, Vector3 spellTargetPosition);
    }
}