using System;
using UnityEngine;

namespace Magie.Spells
{
    public class ProjectileSpellFiringContext : ASpellFiringContext
    { 
        private readonly ProjectileSpell Spell;
        private int _numberOfTriggers;
        private DateTime _lastFiredAt;
        
        public override Spell SpellInUse => Spell;
        
        public ProjectileSpellFiringContext(ProjectileSpell spell, Transform spellOrigin, Action onDepleted) : base(spellOrigin, onDepleted)
        {
            Spell = spell;
            _lastFiredAt = DateTime.MinValue;
        }
        
        public override void TryFire(Transform target, ISpellSpawner spellSpawner)
        {
            if (DateTime.Now - _lastFiredAt < Spell.ShootingCooldown)
            {
                return;
            }

            spellSpawner.SpawnProjectileSpell(Spell.Prefab, SpellOrigin.position, SpellOrigin.rotation, target);

            _lastFiredAt = DateTime.Now;
            _numberOfTriggers++;
            if (_numberOfTriggers >= Spell.NumberOfProjectiles)
            {
                OnDepleted?.Invoke();
            }
        }

        public override void TryCancel()
        {
        }
    }
}