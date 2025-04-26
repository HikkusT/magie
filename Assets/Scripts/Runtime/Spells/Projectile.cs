using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Magie.Spells
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private ParticleSystem _onCollisionVfx;

        private CancellationTokenSource _trajectoryCts = new();

        public async UniTaskVoid PlayTrajectory(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            _trajectoryCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            while (!_trajectoryCts.IsCancellationRequested)
            {
                transform.position += _speed * direction * Time.deltaTime;
                await UniTask.NextFrame();
            }
        }

        private void OnTriggerEnter(Collider _)
        {
            _trajectoryCts.Cancel();
            Instantiate(_onCollisionVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}