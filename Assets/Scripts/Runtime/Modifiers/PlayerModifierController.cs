using UnityEngine;

namespace Modifiers
{
    public class PlayerModifierController : MonoBehaviour
    {
        [SerializeField] private OVRPlayerController _characterController;
        [SerializeField] private float _slowAmount = 0.5f;

        private int _appliedSlowCount;

        public void ApplySlow()
        {
            _characterController.SetMoveScaleMultiplier(_slowAmount);
            _appliedSlowCount++;
        }
        
        public void RemoveSlow()
        {
            _appliedSlowCount--;
            if (_appliedSlowCount == 0)
            {
                _characterController.SetMoveScaleMultiplier(1);
            }
        }
    }
}