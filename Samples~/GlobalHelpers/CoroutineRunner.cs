using System.Collections;
using UnityEngine;

namespace QuickEye.OneAsset.GlobalHelpers
{
    public static class CoroutineRunner
    {
        private static readonly CoroutineRunnerBridge Bridge;

        static CoroutineRunner()
        {
            Bridge = CoroutineRunnerBridge.Instance;
            Bridge.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine) => Bridge.StartCoroutine(coroutine);

        public static void StopCoroutine(IEnumerator coroutine) => Bridge.StopCoroutine(coroutine);

        public static void StopCoroutine(Coroutine coroutine) => Bridge.StopCoroutine(coroutine);

        private class CoroutineRunnerBridge : OneGameObject<CoroutineRunnerBridge>
        {
        }
    }
}