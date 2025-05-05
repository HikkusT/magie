using Unity.Netcode;

namespace Runtime
{
    public class PlayerRigSyncing : NetworkBehaviour
    {
        private OVRCameraRig _rig;
        
        private void Start()
        {
            _rig = FindFirstObjectByType<OVRCameraRig>();
        }

        private void Update()
        {
            if (!IsOwner) return;

            transform.position = _rig.centerEyeAnchor.position;
        }
    }
}