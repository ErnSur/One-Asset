#if !UNITY_EDITOR
#define NO_OPERATION
#else
using System.IO;
using System.Linq;
#endif
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.OneAsset
{
    /// <summary>
    /// Editor features are implemented in the runtime assembly to make sure that they are available when using <see cref="OneAssetLoader"/> from the <see cref="UnityEditor.InitializeOnLoadAttribute"/>.
    /// </summary>
    internal class OneAssetLoaderEditorFeatures : IOneAssetLoaderEditorFeatures
    {
        public bool TryLoadFromAssetDatabase(Type type, AssetPath path, out Object obj)
        {
#if NO_OPERATION
            obj = null;
            return false;
#else
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path.OriginalPath, type);
            return obj != null;
#endif
        }

        public bool TryLoadAndForget(Type type, AssetPath path, out Object obj)
        {
#if NO_OPERATION
            obj = null;
            return false;
#else
            if (!File.Exists(path.OriginalPath))
            {
                obj = null;
                return false;
            }

            obj = UnityEditorInternal.InternalEditorUtility
                .LoadSerializedFileAndForget(path.OriginalPath)
                .FirstOrDefault(o => o.GetType() == type);

            return obj != null;
#endif
        }

        public void CreateAsset(ScriptableObject obj, AssetLoadOptions options)
        {
#if !NO_OPERATION
            var path = options.Paths[0];
            var baseDir = Path.GetDirectoryName(path);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            var assetPath = path;
            if (!assetPath.EndsWith(".asset"))
                assetPath = $"{assetPath}.asset";
            UnityEditor.AssetDatabase.CreateAsset(obj, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}