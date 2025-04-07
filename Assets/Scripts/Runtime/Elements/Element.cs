using UnityEngine;

namespace Magie.Elements
{
    [CreateAssetMenu(menuName = "Magie/Element")]
    public class Element : ScriptableObject
    {
        [SerializeField] private string _debugName;
        [SerializeField] private ParticleSystem _fingertipParticles;
        
        public string DebugName => _debugName;
        public ParticleSystem FingertipParticles => _fingertipParticles;
    }
}