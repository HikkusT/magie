using UnityEngine;
using Unity.Netcode;

namespace Magie.Match
{
    public class PlayerReadyZone : NetworkBehaviour
    {
        private static int PROGRESS_SHADER_PROPERTY_ID = Shader.PropertyToID("_Progress");
        
        [SerializeField] private float _readyTimeInSeconds = 3f;
        [SerializeField] private Renderer _renderer;
        
        public bool IsReady => _progress.Value >= 1f;

        private readonly NetworkVariable<float> _progress = new ();
        private int _playerCountInside;
        private bool _isFrozen;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
 
            _playerCountInside++;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            _playerCountInside--;
        }

        private void Update()
        {
            _renderer.material.SetFloat(PROGRESS_SHADER_PROPERTY_ID, _progress.Value);

            if (IsServer)
            {
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            if (_isFrozen) return;
            
            if (_playerCountInside > 0)
            {
                if (_progress.Value >= 1f) return;
                
                _progress.Value += Time.deltaTime / _readyTimeInSeconds;
            }
            else
            {
                if (_progress.Value <= 0f) return;
                
                _progress.Value -= Time.deltaTime / _readyTimeInSeconds;
            }
            
            _progress.Value = Mathf.Clamp01(_progress.Value);
        }

        public void Freeze()
        {
            _isFrozen = true;

            if (IsServer)
            {
                _progress.Value = 1f;
            }
        }
        
        private void OnDisable()
        {
            if (IsServer)
            {
                _playerCountInside = 0;
                _progress.Value = 0f;
            }

            _isFrozen = false;
        }
    }
} 