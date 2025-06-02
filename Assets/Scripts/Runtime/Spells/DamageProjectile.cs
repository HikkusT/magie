using Multiplayer;
using UnityEngine;

namespace Magie.Spells
{
    public class DamageProjectile : Projectile
    {
        [SerializeField] private int _damage = 1;
        
        protected override void PlayerCollision(Player player)
        {
            player.ReceiveDamageServerRpc(_damage);
        }
    }
}