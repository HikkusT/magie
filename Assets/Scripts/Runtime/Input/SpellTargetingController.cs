using System;
using Magie.Spells;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

namespace Magie.Input
{
    public class SpellTargetingController : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IHand))] private UnityEngine.Object _hand;
        [SerializeField] private Transform magicCenterObject; // Reference object (sphere in the hand)
        [SerializeField] private Transform collisionDetectionPoint; // Sphere to show collision point
        [SerializeField] private LayerMask collisionLayers; // Layers to detect collisions with
        
        [SerializeField] private float offsetPerpendicularDistance = 0.06f; // Distance perpendicular to the hand
        [SerializeField] private float offsetTowardsFingertips = 0.08f; // Distance towards the fingertips
        [SerializeField] private float rayLength = 10f; // Length of the raycast
        [SerializeField] private float speedRotationDetectionPoint = 10f; // Speed of rotation for the collision detection point

        private IHand Hand => _hand as IHand;
        
        private Spell _currentTargetingSpell;

        public void EnterFiringMode(Spell spell)
        {
            Debug.Log($"[Hik] Starting firing mode for spell {spell.name}");
            _currentTargetingSpell = spell;
            magicCenterObject.gameObject.SetActive(true);
            collisionDetectionPoint.gameObject.SetActive(true);
        }

        public void ExitFiringMode()
        {
            _currentTargetingSpell = null;
            magicCenterObject.gameObject.SetActive(false);
            collisionDetectionPoint.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_hand == null || !Hand.IsConnected) return;
            if (_currentTargetingSpell == null) return;
            
            Debug.Log("[hik] Going to get pose");
            Hand.GetRootPose(out Pose palmPose);
            Debug.Log($"[hik] Got pose {palmPose != null}");
            
            magicCenterObject.position = palmPose.position - (palmPose.up * offsetPerpendicularDistance) + (palmPose.forward * offsetTowardsFingertips);;
            magicCenterObject.rotation = palmPose.rotation;
            
            Ray ray = new Ray(palmPose.position, -palmPose.up);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, collisionLayers))
            {
                collisionDetectionPoint.position = hit.point;
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                collisionDetectionPoint.rotation = Quaternion.Slerp(collisionDetectionPoint.rotation, targetRotation, Time.deltaTime * speedRotationDetectionPoint);
            }
        }
    }
}