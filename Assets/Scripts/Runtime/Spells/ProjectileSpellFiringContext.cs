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
        
        public ProjectileSpellFiringContext(ProjectileSpell spell, Action onDepleted) : base(onDepleted)
        {
            Spell = spell;
            _lastFiredAt = DateTime.MinValue;
        }
        
        public override void TryTriggerSpell(Vector3 spellOriginPosition, Vector3 spellTargetPosition)
        {
            if (DateTime.Now - _lastFiredAt < Spell.ShootingCooldown)
            {
                return;
            }

            Projectile projectile = Object.Instantiate(Spell.Prefab, spellOriginPosition, Quaternion.identity);
            projectile.PlayTrajectory(spellTargetPosition).Forget();

            _lastFiredAt = DateTime.Now;
            _numberOfTriggers++;
            if (_numberOfTriggers >= Spell.NumberOfProjectiles)
            {
                OnDepleted?.Invoke();
            }
        }
    }
}