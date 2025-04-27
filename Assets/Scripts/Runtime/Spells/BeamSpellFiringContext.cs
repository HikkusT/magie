using System;
using UnityEngine;

namespace Magie.Spells
{
    public class BeamSpellFiringContext : ASpellFiringContext
    {
        public BeamSpellFiringContext(Action onDepleted) : base(onDepleted)
        {
        }

        public override void TryFire(Vector3 spellOriginPosition, Transform target)
        {
            throw new NotImplementedException();
        }
    }
}