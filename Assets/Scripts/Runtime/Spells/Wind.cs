using System;
using UnityEngine;

namespace Magie.Spells
{
    public class Wind : Construction
    {
        private void OnTriggerStay(Collider other)
        {
            var pushable = other.GetComponentInParent<IPushable>();
            if (pushable == null) return;
            
            pushable.ReceivePush(transform.forward);
        }
    }
}