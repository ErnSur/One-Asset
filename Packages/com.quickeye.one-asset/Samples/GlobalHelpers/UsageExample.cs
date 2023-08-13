using System.Collections;
using UnityEngine;

namespace QuickEye.OneAsset.GlobalHelpers
{
    public class UsageExample
    {
        public UsageExample()
        {
            // ApplicationEvents class allows user to subscribe
            // to MonoBehaviour messages
            // outside of MonoBehaviour class
            ApplicationEvents.ApplicationPaused += OnApplicationPaused;
            
            // CoroutineRunner class allows user to execute coroutines
            // outside of MonoBehaviour class
            CoroutineRunner.StartCoroutine(MyCoroutine());
        }

        private static void OnApplicationPaused(bool paused)
        {
            Debug.Log($"Hello OnApplicationPaused {paused}");
        }
        
        private static IEnumerator MyCoroutine()
        {
            Debug.Log("Hello from...");
            yield return new WaitForSeconds(2);
            Debug.Log("... coroutine!");
        }
    }
}