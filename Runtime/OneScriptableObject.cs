using UnityEngine;

namespace QuickEye.OneAsset
{
    /// <summary>
    /// Adds singleton behaviour to descendant classes.
    /// Loads or creates instance of T when Instance property is used.
    /// Can be combined with <see cref="LoadFromAssetAttribute"/> and <see cref="SettingsProviderAssetAttribute"/>
    /// </summary>
    /// <typeparam name="T">Type of the singleton instance</typeparam>
    public abstract class OneScriptableObject<T> : OneScriptableObject
        where T : OneScriptableObject<T>
    {
        private static T _instance;
        
        /// <summary>
        /// Returns a instance of T.
        /// If no instance of T exists, it will create a new one or load one using <see cref="OneAssetLoader.Load{T}()"/>
        /// </summary>
        public static T Instance => GetInstance();

        private static T GetInstance()
        {
            if (_instance == null)
                _instance = LoadOrCreate();
            return _instance;
        }
        
        private static T LoadOrCreate()
        {
            var asset = OneAssetLoader.Load<T>();
            return asset != null ? asset : CreateScriptableObject();
        }
        
        private static T CreateScriptableObject()
        {
            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;
        }
    }
    

    /// <summary>
    /// Non generic base class of <see cref="OneScriptableObject{T}"/>,
    /// useful for non generic polymorphism.
    /// </summary>
    public abstract class OneScriptableObject : ScriptableObject
    {
    }
}