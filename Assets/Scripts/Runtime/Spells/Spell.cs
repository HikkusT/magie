using System;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class Spell : ScriptableObject
    {
        public abstract ASpellFiringContext CreateContext(Transform spellOrigin, Action onContextClosure);
    }
}