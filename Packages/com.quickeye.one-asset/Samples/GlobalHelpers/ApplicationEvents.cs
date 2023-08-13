using System;
using UnityEngine;

namespace QuickEye.OneAsset.GlobalHelpers
{
    public static class ApplicationEvents
    {
        public static event Action<bool> ApplicationPaused;
        public static event Action<bool> ApplicationFocused;
        public static event Action ApplicationQuit;
        
        static ApplicationEvents()
        {
            var broadcaster = ApplicationEventBroadcaster.Instance;
            broadcaster.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        
        private class ApplicationEventBroadcaster : OneGameObject<ApplicationEventBroadcaster>
        {
            private void OnApplicationPause(bool pauseStatus) => ApplicationPaused?.Invoke(pauseStatus);
            private void OnApplicationFocus(bool hasFocus) => ApplicationFocused?.Invoke(hasFocus);
            protected override void OnApplicationQuit()
            {
                base.OnApplicationQuit();
                ApplicationQuit?.Invoke();
            }
        }
    }
}