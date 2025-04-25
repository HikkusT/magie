using System;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Projectile Spell")]
    public class ProjectileSpell : Spell
    {
        [LayoutGroup("Projectile balance", ELayout.CollapseBox)] 
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _shootingIntervalInSeconds;
        [SerializeField] private bool _isUnlimitedProjectiles;
        [SerializeField, HideIf(nameof(_isUnlimitedProjectiles))] private int _numberOfProjectiles;

        public float ProjectileSpeed => _projectileSpeed;
        public TimeSpan ShootingInterval => TimeSpan.FromSeconds(_shootingIntervalInSeconds);
        public int NumberOfProjectiles => _isUnlimitedProjectiles ? int.MaxValue : _numberOfProjectiles;
    }
}