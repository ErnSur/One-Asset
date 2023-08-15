using EventSystem.Samples.Samples.EventSystem.Samples;
using UnityEngine;

namespace EventSystem.Samples
{
    public class Demo : MonoBehaviour
    {
        private void Awake()
        {
            HpChangedEvent.Subscribe(OnHpChanged);
            
            HpChangedEvent.Invoke(5);
        }

        private static void OnHpChanged(int hp)
        {
            Debug.Log($"Hello, HP changed {hp}");
        }
    }
}
