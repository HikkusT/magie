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

        [SerializeField, Range(0, 1f)] private float _lerpSpeed = 1f;

        private ISpellSpawner SpellSpawner = new SinglePlayerSpellSpawner();
        private ASpellFiringContext _spellFiringContext;

        public void SetupWithNetwork(ISpellSpawner spellSpawner)
        {
            SpellSpawner = spellSpawner;
        }

        public void EnterFiringMode(Spell spell, Action dismissFiringModeTrigger)
        {
            _spellFiringContext = spell.CreateContext(_castingPalm.PalmRoot, onContextClosure: dismissFiringModeTrigger);
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
            
            _spellFiringContext.TryFire(_targetIndicator, SpellSpawner);
        }
        
        public void StopSpell()
        {
            if (_spellFiringContext == null) return;
            
            _spellFiringContext.TryCancel();
        }

        private void Update()
        {
            if (_spellFiringContext == null) return;
            
            Ray ray = new Ray(_castingPalm.PalmRoot.position, -_castingPalm.PalmRoot.up);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _targetCollisionMask))
            {
                _targetIndicator.position = Vector3.Lerp(_targetIndicator.position, hit.point + hit.normal * _hoverOffset, _lerpSpeed);
                _targetIndicator.rotation = Quaternion.Slerp(_targetIndicator.rotation, Quaternion.FromToRotation(Vector3.up, hit.normal), _lerpSpeed);
            }
        }
    }
}