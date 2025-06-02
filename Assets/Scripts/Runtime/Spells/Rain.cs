using UnityEngine;

namespace Magie.Spells
{
    public class Rain : Construction, IPushable
    {
        [SerializeField] private float _pushFactor;
        
        public void ReceivePush(Vector3 direction)
        {
            if (!HasAuthority) return;
            
            transform.position += Vector3.ProjectOnPlane(direction, transform.up) * _pushFactor * Time.fixedDeltaTime;
        }
    }
}