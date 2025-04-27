using System;
using UnityEngine;

namespace Magie.Spells
{
    public class BeamSpellFiringContext : ASpellFiringContext
    {
        private readonly BeamSpell Spell;
        
        public BeamSpellFiringContext(BeamSpell spell, Transform spellOrigin, Action onDepleted) : base(spellOrigin, onDepleted)
        {
            Spell = spell;
        }

        public override void TryFire(Transform target)
        {
            throw new NotImplementedException();
        }

        public override void TryCancel()
        {
            throw new NotImplementedException();
        }
    }
}