using System.Collections.Generic;
using Magie.Elements;
using SaintsField;
using UnityEngine;

namespace Magie.Spells
{
    [CreateAssetMenu(menuName = "Magie/Spell Table")]
    public class SpellTable : ScriptableObject
    {
        [SerializeField] private SaintsDictionary<ElementCombination, Spell> _spells;
        
        public IDictionary<ElementCombination, Spell> LookUp => _spells;
    }
}