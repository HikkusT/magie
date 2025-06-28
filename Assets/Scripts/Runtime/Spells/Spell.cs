using System;
using UnityEngine;

namespace Magie.Spells
{
    public abstract class Spell : ScriptableObject
    {
        [SerializeField] private bool _forceLookToGround;
        
        public bool ForceLookToGround => _forceLookToGround;
        
        public abstract ASpellFiringContext CreateContext(Transform spellOrigin, Action onContextClosure);
    }
}