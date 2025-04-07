using System;
using System.Collections.Generic;
using Magie.Elements;
using Magie.Spells;
using TMPro;
using UnityEngine;

namespace Magie.Input
{
    public class CastingController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _debugText; // TODO: Move debug visualization outside this class
        [SerializeField] private List<CastingFinger> _castingFingers;
        
        private readonly List<string> _castingBuffer = new();

        public void AddToCastingBuffer(Element element)
        {
            _castingBuffer.Add(element.DebugName);
        }

        public void EnterCasting()
        {
            _castingBuffer.Clear();
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

            _castingBuffer.Clear();
            _debugText.text = "Exited casting";

            return null;
        }

        private void Update()
        {
            if (_castingBuffer.Count > 0)
            {
                if (_debugText != null)
                {
                    _debugText.text = string.Join(", ", _castingBuffer);
                }
            }
        }
    }
}