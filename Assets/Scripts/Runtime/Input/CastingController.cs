using System.Collections.Generic;
using System.Linq;
using Magie.Elements;
using Magie.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Magie.Input
{
    public class CastingController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _debugText; // TODO: Move debug visualization outside this class
        [SerializeField] private List<CastingFinger> _castingFingers;
        [SerializeField] private SpellTable _spellTable;
        [SerializeField] private int _maxNumberOfElementsInSpell = 2;
        [SerializeField] private UnityEvent<ElementCombination> _onCastingBufferUpdated;
        
        private ElementCombination _castingBuffer;

        public void AddToCastingBuffer(Element element)
        {
            if (_castingBuffer.Elements.Count >= _maxNumberOfElementsInSpell) return;
            
            _castingBuffer.Add(element);
            _onCastingBufferUpdated.Invoke(_castingBuffer);
        }

        public void EnterCasting()
        {
            _castingBuffer = new(_maxNumberOfElementsInSpell);
            foreach (CastingFinger castingFinger in _castingFingers)
            {
                castingFinger.SetVisibility(true);
                castingFinger.OnCasted += AddToCastingBuffer;
            }
            
            _debugText.text = "Entered casting";
        }

        public Spell FinishCasting()
        {
            foreach (CastingFinger castingFinger in _castingFingers)
            {
                castingFinger.SetVisibility(false);
                castingFinger.OnCasted -= AddToCastingBuffer;
            }

            Debug.Log("[Hik] going to lookup");
            if (!_spellTable.LookUp.TryGetValue(_castingBuffer, out Spell result))
            {
                Debug.LogError($"Failed to find spell for buffer {string.Join(", ", _castingBuffer.Elements.Select(it => it.name))}");
            }
            Debug.Log($"[Hik] got {result != null} {(result != null ? result.name : "none")}");;
            
            _debugText.text = "Exited casting";

            return result;
        }

        public void FlushBuffer()
        {
            _castingBuffer = new();
            _onCastingBufferUpdated.Invoke(_castingBuffer);
        }

        private void Update()
        {
            if (_castingBuffer.Elements != null && _castingBuffer.Elements.Count > 0)
            {
                if (_debugText != null)
                {
                    _debugText.text = string.Join(", ", _castingBuffer.Elements.Select(it => it.name));
                }
            }
        }
    }
}