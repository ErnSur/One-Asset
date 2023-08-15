using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace QuickEye.OneAsset.Editor.UI
{
    internal class AssetMetadata
    {
        public readonly Object Asset;

        public readonly string TypeName;

        public readonly AssetLoadOptions LoadOptions;
        public string FirstLoadPath =>
            LoadOptions.Paths.Length == 0 ? null : LoadOptions.Paths[0];

        /// <summary>
        /// Metadata about the object that has <see cref="LoadFromAssetAttribute"/>
        /// </summary>
        /// <param name="asset">Type of the object has to have the <see cref="LoadFromAssetAttribute"/></param>
        public AssetMetadata(Object asset)
        {
            Asset = asset;
            var type = asset.GetType();
            TypeName = type.Name;
            LoadOptions = AssetLoadOptionsUtility.GetLoadOptions(type);
        }
        
        
        public bool IsInLoadablePath(out AssetPath loadPath)
        {
            var assetPath = AssetDatabase.GetAssetPath(Asset);
            return LoadOptions.TryGetLoadAssetPath(assetPath, out loadPath);
        }
    }
}