using System.Collections.Generic;
using Magie.Spells;
using UnityEngine;

namespace Magie.Input
{
    public class SpellCastingInputController : MonoBehaviour
    {
        public enum State
        {
            None,
            Casting,
            Firing
        }

        [SerializeField] private CastingController _castingController;
        
        private State _currentState = State.None;
        
        public void TryEnterCastingState() => TryToTransitionTo(State.Casting);
        public void ResetState() => TryToTransitionTo(State.None);

        public void TryToTransitionTo(State updatedState)
        {
            if (_currentState == updatedState) return;
            
            // TODO: Formalize state machine
            switch (_currentState)
            {
                case State.None:
                {
                    if (updatedState == State.Firing) return;
                    
                    _castingController.EnterCasting();
                    break;
                }
                case State.Casting:
                {
                    Spell result = _castingController.FinishCasting();

                    if (updatedState == State.Firing)
                    {
                        // TODO: Link with target system
                    }
                    break;
                }
            }

            _currentState = updatedState;
        }
    }
}
