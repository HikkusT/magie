using UnityEngine;
using Bhaptics.SDK2;

namespace Magie.Input
{
    public class ControllerBasedSpellFiringTrigger : MonoBehaviour
    {
        [SerializeField] private SpellFiringController _spellFiringController;
        
        private bool _isFiring;
        
        private void Update()
        {
            float leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            
            if (leftTrigger > 0.5f && !_isFiring)
            {
                _isFiring = true;
                BhapticsLibrary.Play("firing");
                _spellFiringController.FireSpell();
            }

            else if (leftTrigger <= 0.5f && _isFiring)
            {
                _isFiring = false;
                _spellFiringController.StopSpell();
            }
        }
    }
}