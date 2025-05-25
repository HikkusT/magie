using System;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Runtime
{
    public class PlayerRigSyncing : NetworkBehaviour
    {
        private static int IS_WALKING_PARAMETER_ID = Animator.StringToHash("IsWalking");
        private static int DELTA_FORWARD_PARAMETER_ID = Animator.StringToHash("DeltaForward");
        private static int DELTA_SIDEWAYS_PARAMETER_ID = Animator.StringToHash("DeltaSideways");
        
        [SerializeField] private Renderer _characterRenderer;
        [SerializeField] private Collider _characterCollider;
        [SerializeField] private Animator _characterAnimator;
        
        [SerializeField] private MappedTransform _headTransform;
        [SerializeField] private MappedTransform _leftArmTransform;
        [SerializeField] private MappedTransform _rightArmTransform;
        
        [SerializeField] private float _minimumWalkingSpeed = 0.05f;
        
        private OVRCameraRig _rig;
        private Vector3 _bodySize;
        private Vector3 _lastTrackedPosition;
        private Vector3 _lastTrackedVelocity;
        
        private void Start()
        {
            _rig = FindFirstObjectByType<OVRCameraRig>();
            _bodySize = _headTransform.Transform.position - transform.position;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                _characterRenderer.enabled = false;
                _characterCollider.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                
                if (Application.isEditor)
                {
                    _characterRenderer.enabled = true;
                    _characterRenderer.gameObject.SetLayerRecursively(LayerMask.NameToLayer("EditorOnly"));
                }
            }

            _lastTrackedPosition = _headTransform.Transform.position;
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            transform.position = _headTransform.Transform.position - _bodySize;
            transform.forward = Vector3.ProjectOnPlane(_headTransform.Transform.forward, Vector3.up);
            
            _headTransform.Update(_rig.centerEyeAnchor);
            _leftArmTransform.Update(_rig.leftHandAnchor);
            _rightArmTransform.Update(_rig.rightHandAnchor);
        }

        private void Update()
        {
            Vector3 playerVelocity = (_headTransform.Transform.position - _lastTrackedPosition) / Time.deltaTime;
            playerVelocity.y = 0;
            playerVelocity = transform.InverseTransformDirection(playerVelocity);
            
            _characterAnimator.SetBool(IS_WALKING_PARAMETER_ID, playerVelocity.magnitude > _minimumWalkingSpeed);
            _characterAnimator.SetFloat(DELTA_FORWARD_PARAMETER_ID, playerVelocity.z);
            _characterAnimator.SetFloat(DELTA_SIDEWAYS_PARAMETER_ID, playerVelocity.x);
            
            _lastTrackedPosition = _headTransform.Transform.position;
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