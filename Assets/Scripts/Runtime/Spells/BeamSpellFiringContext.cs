using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magie.Spells
{
    public class BeamSpellFiringContext : ASpellFiringContext
    {
        private readonly BeamSpell Spell;
        private readonly CancellationTokenSource _cts = new();
        private bool _isFiring;
        
        public BeamSpellFiringContext(BeamSpell spell, Transform spellOrigin, Action onDepleted) : base(spellOrigin, onDepleted)
        {
            Spell = spell;
        }

        public override void TryFire(Transform target, ISpellSpawner spellSpawner)
        {
            if (_isFiring) return;

            FireBeam(target).Forget();
        }

        private async UniTaskVoid FireBeam(Transform target)
        {
            _isFiring = true;
            DateTime startedAt = DateTime.Now;

            Beam beam = Object.Instantiate(Spell.Prefab);
            beam.Setup(SpellOrigin, target);
            await UniTask
                .WaitWhile(() => (DateTime.Now - startedAt) < Spell.MaxBeamDuration, cancellationToken: _cts.Token)
                .SuppressCancellationThrow();

            Object.Destroy(beam.gameObject);
            OnDepleted?.Invoke();
            _isFiring = false;
        }

        public override void TryCancel()
        {
            if (!_isFiring) return;
            
            _cts.Cancel();
        }
    }
}