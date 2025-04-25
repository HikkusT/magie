using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
        
        public string Name => _name;
        public GameObject Prefab => _prefab;
    }
    
    
}