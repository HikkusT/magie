using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Magie.Spells
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _onCollisionVfx;

        private CancellationTokenSource _trajectoryCts = new();

        public void PlayTrajectory(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.linearVelocity = direction * _speed;
        }

        private void OnTriggerEnter(Collider _)
        {
            _trajectoryCts.Cancel();
            Instantiate(_onCollisionVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}