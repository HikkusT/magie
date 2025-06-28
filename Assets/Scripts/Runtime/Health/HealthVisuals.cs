using Microlight.MicroBar;
using Multiplayer;
using UnityEngine;
using Bhaptics.SDK2;
using System;

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

        DateTime _lastPlayedHaptic = DateTime.MaxValue;

        private void Update()
        {
            if (_player.CurrentHealth.Value > 4 || _player.CurrentHealth.Value <= 0) return;

            TimeSpan delayToPlay = _player.CurrentHealth.Value > 1 ? TimeSpan.FromSeconds(2) : TimeSpan.FromSeconds(1.3f); 
            if (DateTime.UtcNow > _lastPlayedHaptic + delayToPlay)
            {
                BhapticsLibrary.Play("entered_casting", 0, 1, 2, 0, 0);
                _lastPlayedHaptic = DateTime.UtcNow;
            }
        }

        private void UpdateHealth(int previousHealth, int updatedHealth)
        {
            _healthBar.UpdateBar(updatedHealth);
            _healthBar.gameObject.SetActive(updatedHealth > 0);

            if (updatedHealth < previousHealth && _player.IsOwner)
            {
                BhapticsLibrary.Play("entered_casting", 0, 1, 2, 0, 0);
                _lastPlayedHaptic = DateTime.UtcNow;
            }

            //Debug.Log($"[HealthVisuals] Previous health: {previousHealth}, Updated health: {updatedHealth}");

        //     if (updatedHealth < previousHealth)
        //     {
                
        //         BhapticsLibrary.Play("entered_casting", 0, 1, 2, 0, 0);

        //         if ((updatedHealth == 4) || (updatedHealth == 3))
        //         {
        //             BhapticsLibrary.PlayLoop("entered_casting", 0, 1, 1f, 0, 0);
        //         }

        //         if ((updatedHealth == 3) || (updatedHealth == 2))
        //         {
        //             BhapticsLibrary.PlayLoop("entered_casting", 0, 1, 0.5f, 0, 0);
        //         }

        //         if (updatedHealth <= 1)                 
        //         {
        //             BhapticsLibrary.Play("fire", 0, 1, 2, 0, 0);
        //         }

        //     }
        }
    }
}
