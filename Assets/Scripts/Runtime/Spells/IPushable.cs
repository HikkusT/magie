using UnityEngine;

namespace Magie.Spells
{
    public interface IPushable
    {
        void ReceivePush(Vector3 direction);
    }
}