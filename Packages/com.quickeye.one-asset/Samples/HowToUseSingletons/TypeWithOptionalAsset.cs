using UnityEngine;

namespace QuickEye.OneAsset.HowToUseSingletons
{
    /// <summary>
    /// Load Asset from Resources folder Example
    /// Singleton with path from which it should be loaded from.
    /// </summary>
    [LoadFromAsset(ResourcesPath, AssetIsMandatory = false)]
    public class TypeWithOptionalAsset : OneScriptableObject<TypeWithOptionalAsset>
    {
        private const string ResourcesPath = "Assets/Resources/OptionalAsset";

        [TextArea]
        public string Description =
            $"Call to `{nameof(TypeWithOptionalAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(LoadFromAssetAttribute)}.`" +
            $"If there is no asset at that path and `{nameof(LoadFromAssetAttribute)}.{nameof(LoadFromAssetAttribute.AssetIsMandatory)}` is set to `true` it will throw an exception, otherwise it will create a new non-asset instance.";
    }
}