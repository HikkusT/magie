using System.Collections.Generic;
using Magie.Elements;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magie.Input
{
    public class CastingPalm : MonoBehaviour
    {
        [LayoutGroup("Hand Config", ELayout.CollapseBox)]
        [SerializeField, Interface(typeof(IHand))] private Object _hand;
        [SerializeField] private float _offsetPerpendicularDistance = 0.06f;
        [SerializeField] private float _offsetTowardsFingertips = 0.08f;
        [LayoutEnd]
        
        [SerializeField] private SaintsDictionary<Element, GameObject> _castingElementVfxPrefabs;
        [SerializeField] private float _vfxVerticalOffset;
        
        public Transform PalmRoot { get; private set; }
        private IHand Hand => _hand as IHand;
        private readonly Dictionary<Element, GameObject> _instantiatedCastingElementVfxs = new();

        private void Start()
        {
            PalmRoot = new GameObject("PalmRoot").transform;
            foreach ((Element element, GameObject prefab) in _castingElementVfxPrefabs)
            {
                _instantiatedCastingElementVfxs[element] = Instantiate(prefab, Vector3.up * _vfxVerticalOffset, Quaternion.identity, PalmRoot);
                _instantiatedCastingElementVfxs[element].SetActive(false);
            }
        }

        public void RefreshVfxs(ElementCombination elementCombination)
        {
            foreach ((Element element, GameObject vfx) in _instantiatedCastingElementVfxs)
            {
                vfx.SetActive(elementCombination.Elements?.Contains(element) == true);
            }
        }

        private void Update()
        {
            if (Hand is not { IsConnected: true }) return;
            
            Hand.GetRootPose(out Pose palmPose);
            PalmRoot.position = palmPose.position - (palmPose.up * _offsetPerpendicularDistance) + (palmPose.forward * _offsetTowardsFingertips);;
            PalmRoot.rotation = palmPose.rotation;
        }
    }
}