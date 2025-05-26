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
            _healthBar.gameObject.SetActive(player.CurrentHealth.Value > 0);
            player.CurrentHealth.OnValueChanged += UpdateHealth;
        }

        private void UpdateHealth(int previousHealth, int updatedHealth)
        {
            _healthBar.UpdateBar(updatedHealth);

            _healthBar.gameObject.SetActive(updatedHealth > 0);
        }
    }
}