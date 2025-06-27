using Health;
using Runtime;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multiplayer
{
    public class Player : NetworkBehaviour
    {
        [field: SerializeField] public int InitialHealth { get; private set; }

        [SerializeField] private HealthVisuals _healthVisuals;
        [SerializeField] private HealthVisuals _healthVisualsArm;
        [SerializeField] private GameObject _visualRoot;
        [SerializeField] private ParticleSystem _hitVfx;

        public readonly NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

        private int? _fallDamage;

        public override void OnNetworkSpawn()
        {
            _healthVisuals.Setup(this);

            if (IsOwner && _healthVisualsArm != null)
            {
                _healthVisualsArm.Setup(this);
                _healthVisualsArm.gameObject.SetActive(true);
            }
            else if (_healthVisualsArm != null)
            {
                _healthVisualsArm.gameObject.SetActive(false);
            }

        }

        private void Update()
        {
            if (IsOwner && Keyboard.current.lKey.wasPressedThisFrame)
            {
                ReceiveDamageServerRpc(1);
            }

            if (IsOwner && _fallDamage.HasValue && transform.position.y < 0.5f)
            {
                ReceiveDamageServerRpc(_fallDamage.Value);
                _fallDamage = null;
            }
        }

        public void InitializeForCombat()
        {
            CurrentHealth.Value = InitialHealth;
        }

        public void SetupFallDamage(int damage)
        {
            _fallDamage = damage;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReceiveDamageServerRpc(int damage)
        {
            if (CurrentHealth.Value <= 0) return;
            
            CurrentHealth.Value -= damage;
            PlayHitVfxClientRpc();
        }

        [ClientRpc]
        private void PlayHitVfxClientRpc()
        {
            Instantiate(_hitVfx, _visualRoot.transform.position + 3 * Vector3.up, Quaternion.identity);
        }
    }
}