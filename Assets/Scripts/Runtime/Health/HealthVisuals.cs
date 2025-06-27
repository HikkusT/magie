using Microlight.MicroBar;
using Multiplayer;
using UnityEngine;
using Bhaptics.SDK2;

namespace Health
{
    public class HealthVisuals : MonoBehaviour
    {
        [SerializeField] private MicroBar _healthBar;
        private Player _player;

        public void Setup(Player player)
        {
            _player = player;
            _healthBar.Initialize(player.InitialHealth);
            _healthBar.gameObject.SetActive(player.CurrentHealth.Value > 0);
            player.CurrentHealth.OnValueChanged += UpdateHealth;
        }

        private void UpdateHealth(int previousHealth, int updatedHealth)
        {
            _healthBar.UpdateBar(updatedHealth);
            _healthBar.gameObject.SetActive(updatedHealth > 0);

            //Debug.Log($"[HealthVisuals] Previous health: {previousHealth}, Updated health: {updatedHealth}");

            if (updatedHealth < previousHealth)
            {
                
                BhapticsLibrary.Play("entered_casting", 0, 1, 3, 0, 0);




            }
        }
    }
}
