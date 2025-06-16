using System;
using UnityEngine;

namespace Utils
{
    public class CollisionEventRaiser : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }
    }
}