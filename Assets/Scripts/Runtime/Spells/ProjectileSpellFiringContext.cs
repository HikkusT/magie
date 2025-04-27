using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magie.Spells
{
    public class ProjectileSpellFiringContext : ASpellFiringContext
    { 
        private readonly ProjectileSpell Spell;
        private int _numberOfTriggers;
        private DateTime _lastFiredAt;
        
        public ProjectileSpellFiringContext(ProjectileSpell spell, Transform spellOrigin, Action onDepleted) : base(spellOrigin, onDepleted)
        {
            Spell = spell;
            _lastFiredAt = DateTime.MinValue;
        }
        
        public override void TryFire(Transform target)
        {
            if (DateTime.Now - _lastFiredAt < Spell.ShootingCooldown)
            {
                return;
            }

            Projectile projectile = Object.Instantiate(Spell.Prefab, SpellOrigin.position, Quaternion.identity);
            projectile.PlayTrajectory(target.position).Forget();

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