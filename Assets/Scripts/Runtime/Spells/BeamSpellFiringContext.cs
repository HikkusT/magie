using System;
using UnityEngine;

namespace Magie.Spells
{
    public class BeamSpellFiringContext : ASpellFiringContext
    {
        public BeamSpellFiringContext(Action onDepleted) : base(onDepleted)
        {
        }

        public override void TryTriggerSpell(Vector3 spellOriginPosition, Vector3 spellTargetPosition)
        {
            throw new NotImplementedException();
        }
    }
}