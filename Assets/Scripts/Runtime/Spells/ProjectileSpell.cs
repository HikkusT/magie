using System;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Projectile Spell")]
    public class ProjectileSpell : Spell
    {
        [SerializeField] private Projectile _prefab;
        
        [LayoutGroup("Projectile balance", ELayout.CollapseBox)] 
        [SerializeField] private float _shootingCooldownInSeconds;
        [SerializeField] private bool _isUnlimitedProjectiles;
        [SerializeField, HideIf(nameof(_isUnlimitedProjectiles))] private int _numberOfProjectiles;

        public Projectile Prefab => _prefab;
        public TimeSpan ShootingCooldown => TimeSpan.FromSeconds(_shootingCooldownInSeconds);
        public int NumberOfProjectiles => _isUnlimitedProjectiles ? int.MaxValue : _numberOfProjectiles;

        public override ASpellFiringContext CreateContext(Action onContextClosure) =>
            new ProjectileSpellFiringContext(this, onContextClosure);
    }
}