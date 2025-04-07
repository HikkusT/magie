using System;
using System.Collections.Generic;
using Magie.Spells;
using TMPro;
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
        
        public enum StateTransition
        {
            StartCasting,
            DismissCasting,
            FinishCasting,
            DismissFiring
        }

        [SerializeField] private CastingController _castingController;
        [SerializeField] private SpellCasting _firingController;
        [SerializeField] private TMP_Text _debugText;
        
        private State _currentState = State.None;

        public void ApplyStartCastingTransition() => ApplyTransition(StateTransition.StartCasting);
        public void ApplyDismissCastingTransition() => ApplyTransition(StateTransition.DismissCasting);
        public void ApplyFinishCastingTransition() => ApplyTransition(StateTransition.FinishCasting);
        public void ApplyDismissFiringTransition() => ApplyTransition(StateTransition.DismissFiring);
        
        
        public void ApplyTransition(StateTransition transition)
        {
            // TODO: Formalize state machine
            switch (transition)
            {
                case StateTransition.StartCasting when _currentState == State.None:
                {
                    _castingController.EnterCasting();
                    _currentState = State.Casting;
                    break;
                }
                case StateTransition.DismissCasting when _currentState == State.Casting:
                {
                    _castingController.FinishCasting();
                    _currentState = State.None;
                    break;
                }
                case StateTransition.FinishCasting when _currentState == State.Casting:
                {
                    Spell result = _castingController.FinishCasting();
                    _firingController.gameObject.SetActive(true);
                    _currentState = State.Firing;
                    // TODO: Link with firing controller
                    break;
                }
                case StateTransition.DismissFiring when _currentState == State.Firing:
                {
                    _currentState = State.None;
                    _firingController.gameObject.SetActive(false);
                    break;
                }
            }
        }

        private void Update()
        {
            if (_debugText != null)
            {
                _debugText.text = _currentState.ToString();
            }
        }
    }
}
