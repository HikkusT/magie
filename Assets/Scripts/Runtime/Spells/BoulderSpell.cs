﻿using UnityEngine;

namespace Magie.Spells
{
    public class BoulderSpell : ConstructionSpell
    {
        [SerializeField] private float _offset;
        [SerializeField] private float _groundOffset;
        
        public override Vector3 CalculateSpawnPosition(Transform origin, Transform target)
        {
            Vector3 offsetPosition = origin.position + Vector3.ProjectOnPlane(target.position - origin.position, Vector3.up).normalized * _offset;
            
            return new (offsetPosition.x, _groundOffset, offsetPosition.z);
        }
    }
}