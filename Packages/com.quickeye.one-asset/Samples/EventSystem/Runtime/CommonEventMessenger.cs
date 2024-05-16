using System;
using UnityEngine;

namespace QuickEye.EventSystem
{
    public class CommonEventMessenger : MonoBehaviour
    {
        public event Action Destroyed;
        public event Action Disabled;

        void OnDestroy() => Destroyed?.Invoke();
        void OnDisable() => Disabled?.Invoke();
    }
}