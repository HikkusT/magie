using System;
using SaintsField.Playa;
using UnityEngine;

namespace Magie.Spells
{
    public class Beam : MonoBehaviour
    {
        [LayoutGroup("Prefabs", ELayout.CollapseBox)]
        [SerializeField] private GameObject _beamLinePrefab;
        [SerializeField] private GameObject _beamStartPrefab;
        [SerializeField] private GameObject _beamEndPrefab;
        
        [LayoutGroup("Beam config", ELayout.CollapseBox)]
        [SerializeField] private float _beamWidth = 1f; 
        [SerializeField] private float _beamWidthMultiplier = 1f;
        [SerializeField] private float _pulseSpeed = 1.0f;
        [SerializeField] private float _textureScrollSpeed = 0f; 
        [SerializeField] private float _textureLengthScale = 1f;
        [LayoutEnd] 
        
        [SerializeField] private float _startBeamOffset;

        private Transform _origin;
        private Transform _end;
        
        private GameObject _beam;
        private GameObject _beamStart;
        private GameObject _beamEnd;
        private LineRenderer _line;

        private bool _isExpanding = true;
        private float _lerpValue;
        
        public void Setup(Transform origin, Transform end)
        {
	        _origin = origin;
	        _end = end;
	        
            _beam = Instantiate(_beamLinePrefab);
            _beam.transform.position = transform.position;
            _beam.transform.parent = transform;
            _beam.transform.rotation = transform.rotation;

            _line = _beam.GetComponent<LineRenderer>();
            _line.useWorldSpace = true;

#if UNITY_5_5_OR_NEWER
            _line.positionCount = 2;
#else
			_line.SetVertexCount(2); 
#endif

            _beamStart = Instantiate(_beamStartPrefab, _beam.transform);
            _beamEnd = Instantiate(_beamEndPrefab, _beam.transform);
            _beamStart.transform.localScale = 0.1f * Vector3.one;
            _beamEnd.transform.localScale = 0.1f * Vector3.one;
        }

        private void FixedUpdate()
        {
	        if (_beam == null) return;
	        
	        _beamStart.transform.position = _origin.position + _startBeamOffset * -_origin.up;
	        _beamEnd.transform.position = _end.transform.position;
	        transform.rotation = Quaternion.LookRotation(_beamEnd.transform.position - _beamStart.transform.position);
	        _line.SetPosition(0, _beamStart.transform.position);
	        _line.SetPosition(1, _beamEnd.transform.position);
        }

        private void Update()
        {
	        if (_beam == null) return;
	        
	        float distance = Vector3.Distance(_beamEnd.transform.position, _beamStart.transform.position);
	        _line.material.mainTextureScale = new Vector2(distance / _textureLengthScale, 1);
	        _line.material.mainTextureOffset -= new Vector2(Time.deltaTime * _textureScrollSpeed, 0);
	        
	        if (_isExpanding) 
	        {
		        _lerpValue += Time.deltaTime * _pulseSpeed;
	        } 
	        else 
	        {
		        _lerpValue -= Time.deltaTime * _pulseSpeed;
	        }
	        
	        if (_lerpValue >= 1.0f) 
	        {
		        _isExpanding = false;
		        _lerpValue = 1.0f;
	        } 
	        else if (_lerpValue <= 0.0f) 
	        {
		        _isExpanding = true;
		        _lerpValue = 0.0f;
	        }
	        
	        float currentWidth = Mathf.Lerp(_beamWidth, _beamWidth * _beamWidthMultiplier, Mathf.Sin(_lerpValue * Mathf.PI));
	        
	        _line.startWidth = currentWidth;
	        _line.endWidth = currentWidth;
        }
    }
}