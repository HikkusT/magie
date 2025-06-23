using Multiplayer;
using UnityEngine;

namespace Magie.Spells
{
    public class Tornado : MonoBehaviour, IPushable
    {
        [SerializeField] private float _lift;
        [SerializeField] private float _pushFactor;

        
        private void OnTriggerStay(Collider other)
        {
            Player collidedPlayer = other.GetComponentInParent<Player>();
            if (collidedPlayer == null) return;
                
            var characterController = FindObjectOfType<OVRPlayerController>().GetComponent<CharacterController>();
            characterController.Move(Vector3.up * _lift * Time.fixedDeltaTime);
        }
        
        public void ReceivePush(Vector3 direction)
        {
            // if (!HasAuthority) return;
            
            transform.position += Vector3.ProjectOnPlane(direction, transform.up) * _pushFactor * Time.fixedDeltaTime;
        }
    }
}