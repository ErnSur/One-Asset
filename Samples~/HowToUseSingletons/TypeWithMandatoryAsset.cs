using UnityEngine;

namespace QuickEye.OneAsset.HowToUseSingletons
{
    [LoadFromAsset(Path, AssetIsMandatory = true, CreateAssetIfMissing = true)]
    public class TypeWithMandatoryAsset : OneScriptableObject<TypeWithMandatoryAsset>
    {
        private const string Path = "Assets/Samples/Settings/Resources/" + nameof(TypeWithMandatoryAsset)+".asset";

        [TextArea(10,30)]
        public string Description =
            $"Call to `{nameof(TypeWithMandatoryAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(LoadFromAssetAttribute)}.`" +
            $"If there is no asset at that path a new Asset will be created at: {Path}";
    }
}