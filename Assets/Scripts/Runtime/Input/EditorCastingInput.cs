using SaintsField;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Magie.Input
{
    public class EditorCastingInput : MonoBehaviour
    {
        [SerializeField] private SpellCastingInputController _inputController;
        [SerializeField] private SpellFiringController _firingController;
        [SerializeField] private SaintsDictionary<CastingFinger, Key> _keysByFinger;
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                _inputController.ApplyStartCastingTransition();
            }

            foreach ((CastingFinger finger, Key key) in _keysByFinger)
            {
                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    finger.TriggerCasted();
                }
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                _inputController.ApplyFinishCastingTransition();
            }
            
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _inputController.ApplyDismissFiringTransition();
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                //_firingController.FireSpell();
            }
            
            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                //_firingController.StopSpell();
            }
        }
        #endif
    }
}