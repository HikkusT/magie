using System;
using Magie.Spells;
using UnityEngine;

namespace Magie.Input
{
    public class SpellFiringController : MonoBehaviour
    {
        [SerializeField] private CastingPalm _castingPalm;

        [SerializeField] private Transform _targetIndicator;
        [SerializeField] private LayerMask _targetCollisionMask;
        [SerializeField] private float _hoverOffset;
        
        private ASpellFiringContext _spellFiringContext;

        public void EnterFiringMode(Spell spell, Action dismissFiringModeTrigger)
        {
            _spellFiringContext = spell.CreateContext(onContextClosure: dismissFiringModeTrigger);
            _targetIndicator.gameObject.SetActive(true);
        }

        public void ExitFiringMode()
        {
            _spellFiringContext = null;
            _targetIndicator.gameObject.SetActive(false);
        }

        public void FireSpell()
        {
            if (_spellFiringContext == null) return;
            
            _spellFiringContext.TryTriggerSpell(_castingPalm.PalmRoot.position, _targetIndicator.position);
        }

        private void Update()
        {
            if (_spellFiringContext == null) return;
            
            Ray ray = new Ray(_castingPalm.PalmRoot.position, -_castingPalm.PalmRoot.up);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _targetCollisionMask))
            {
                _targetIndicator.position = hit.point + hit.normal * _hoverOffset;
                _targetIndicator.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
}