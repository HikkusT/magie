using UnityEngine;

namespace Magie.Spells
{
    public class TeleportProjectile : Projectile
    {
        protected override void TerrainCollision()
        {
            var player = FindFirstObjectByType<CharacterController>();
            var cameraRig = player.GetComponentInChildren<OVRCameraRig>();

            Vector3 delta = cameraRig.centerEyeAnchor.position - player.transform.position;
            Vector3 targetPos = transform.position - delta;
            player.enabled = false;
            player.transform.position = new Vector3(targetPos.x, 0, targetPos.z);
            player.enabled = true;
        }
    }
}