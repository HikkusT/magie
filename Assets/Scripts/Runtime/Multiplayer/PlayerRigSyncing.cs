using System;
using Unity.Netcode;
using UnityEngine;

namespace Runtime
{
    public class PlayerRigSyncing : NetworkBehaviour
    {
        [SerializeField] private MappedTransform _headTransform;
        [SerializeField] private MappedTransform _leftArmTransform;
        [SerializeField] private MappedTransform _rightArmTransform;
        
        private OVRCameraRig _rig;
        private Vector3 _bodySize;
        
        private void Start()
        {
            _rig = FindFirstObjectByType<OVRCameraRig>();
            _bodySize = _headTransform.Transform.position - transform.position;
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            transform.position = _headTransform.Transform.position - _bodySize;
            transform.rotation = _headTransform.Transform.rotation;
            
            _headTransform.Update(_rig.centerEyeAnchor);
            _leftArmTransform.Update(_rig.leftHandAnchor);
            _rightArmTransform.Update(_rig.rightHandAnchor);
        }
        
        [Serializable]
        private struct MappedTransform
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public Vector3 PositionOffset { get; private set; }
            [field: SerializeField] public Vector3 RotationOffset { get; private set; }

            public void Update(Transform reference)
            {
                Transform.position = reference.position + PositionOffset;
                Transform.rotation = reference.rotation * Quaternion.Euler(RotationOffset);
            }
        } 
    }
}