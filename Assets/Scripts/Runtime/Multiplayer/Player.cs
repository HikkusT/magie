using Health;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public class Player : NetworkBehaviour
    {
        [field: SerializeField] public int InitialHealth { get; private set; }

        [SerializeField] private HealthVisuals _healthVisuals;

        public readonly NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                CurrentHealth.Value = InitialHealth;
            }
            _healthVisuals.Setup(this);
        }

        [ServerRpc]
        private void ReceiveDamageServerRpc(int damage)
        {
            CurrentHealth.Value -= damage;
        }
    }
}