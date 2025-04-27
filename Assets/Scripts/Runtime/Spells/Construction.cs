using UnityEngine;

namespace Magie.Spells
{
    public class Construction : MonoBehaviour
    {
        [SerializeField] private float _ttlInSeconds;

        private void Start()
        {
            Destroy(gameObject, _ttlInSeconds);
        }
    }
}