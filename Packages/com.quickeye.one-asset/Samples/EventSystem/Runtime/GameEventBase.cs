using QuickEye.OneAsset;
using UnityEngine;

namespace QuickEye.EventSystem
{
    public abstract class GameEventBase : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea]
        [SerializeField]
        protected internal string developerDescription;
#endif
        [SerializeField]
        protected bool wasInvoked;

        public bool WasInvoked => wasInvoked;
        
        protected virtual void OnEnable() => ResetValues();
        protected virtual void OnDisable() => ResetValues();

        protected virtual void ResetValues()
        {
            wasInvoked = false;
        }
        
        public static T LoadOrCreate<T>() where T : GameEventBase
        {
            var loadOptions = new AssetLoadOptions($"Assets/Resources/GameEvents/{typeof(T).Name}.asset")
            {
                CreateAssetIfMissing = true
            };
            var asset = OneAssetLoader.Load<T>(loadOptions);
            if (asset != null)
                return asset;
            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;
        }
    }

    // just for polymorphic method invocation in Editor 
    internal interface IInvokable
    {
        void RepeatLastInvoke();
    }
}