using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.OneAsset
{
    internal interface IOneAssetLoaderEditorFeatures
    {
        void CreateAsset(ScriptableObject scriptableObject, AssetLoadOptions options);
        
        bool TryLoadFromAssetDatabase(Type type, AssetPath path, out Object obj);
        
        bool TryLoadAndForget(Type type, AssetPath path, out Object obj);
    }
}