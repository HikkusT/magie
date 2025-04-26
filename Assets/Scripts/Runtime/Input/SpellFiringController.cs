using System;
using Magie.Spells;
using UnityEngine;

namespace Magie.Input
{
    public class SpellFiringController : MonoBehaviour
    {
        [SerializeField] private CastingPalm _castingPalm;
        [SerializeField] private Transform collisionDetectionPoint;
        [SerializeField] private LayerMask collisionLayers;
        
        [SerializeField] private float rayLength = 10f; // Length of the raycast
        [SerializeField] private float speedRotationDetectionPoint = 10f; // Speed of rotation for the collision detection point
        
        private ASpellFiringContext _spellFiringContext;

        public void EnterFiringMode(Spell spell, Action dismissFiringModeTrigger)
        {
            _spellFiringContext = spell.CreateContext(onContextClosure: dismissFiringModeTrigger);
            collisionDetectionPoint.gameObject.SetActive(true);
        }

        public void ExitFiringMode()
        {
            _spellFiringContext = null;
            collisionDetectionPoint.gameObject.SetActive(false);
        }

        public void FireSpell()
        {
            if (_spellFiringContext == null) return;
            
            _spellFiringContext.TryTriggerSpell(_castingPalm.PalmRoot.position, collisionDetectionPoint.position);
        }

        private void Update()
        {
            if (_spellFiringContext == null) return;
            
            Ray ray = new Ray(_castingPalm.PalmRoot.position, -_castingPalm.PalmRoot.up);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, collisionLayers))
            {
                collisionDetectionPoint.position = hit.point;
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                collisionDetectionPoint.rotation = Quaternion.Slerp(collisionDetectionPoint.rotation, targetRotation, Time.deltaTime * speedRotationDetectionPoint);
            }
        }
    }
}