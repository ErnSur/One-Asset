using System;
using UnityEngine;

namespace QuickEye.EventSystem
{
    public class CommonEventMessenger : MonoBehaviour
    {
        public event Action Destroyed;
        public event Action Disabled;

        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        void OnDestroy() => Destroyed?.Invoke();
        void OnDisable() => Disabled?.Invoke();
    }
}