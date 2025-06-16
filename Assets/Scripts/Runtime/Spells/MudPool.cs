using System.Linq;
using Modifiers;
using UnityEngine;

namespace Magie.Spells
{
    public class MudPool : MonoBehaviour
    {
        private bool _isLocalPlayerInside;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("LocalPlayer")) return;
            
            FindObjectsByType<PlayerModifierController>(FindObjectsSortMode.None).First().ApplySlow();
            _isLocalPlayerInside = true;
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("LocalPlayer")) return;
            if (!_isLocalPlayerInside) return;
            
            FindObjectsByType<PlayerModifierController>(FindObjectsSortMode.None).First().RemoveSlow();
            _isLocalPlayerInside = false;
        }

        private void OnDestroy()
        {
            if (_isLocalPlayerInside)
            {
                FindObjectsByType<PlayerModifierController>(FindObjectsSortMode.None).First().RemoveSlow();
                _isLocalPlayerInside = false;
            }
        }
    }
}