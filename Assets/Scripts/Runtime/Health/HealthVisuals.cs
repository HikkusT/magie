using Microlight.MicroBar;
using Multiplayer;
using UnityEngine;

namespace Health
{
    public class HealthVisuals : MonoBehaviour
    {
        [SerializeField] private MicroBar _healthBar;

        public void Setup(Player player)
        {
            _healthBar.Initialize(player.InitialHealth);
            player.CurrentHealth.OnValueChanged += UpdateHealth;
        }

        private void UpdateHealth(int previousHealth, int updatedHealth)
        {
            _healthBar.UpdateBar(updatedHealth);
        }
    }
}