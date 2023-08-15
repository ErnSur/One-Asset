namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Resources/" + ResourcesPath + ".asset")]
    internal class OsoWithAsset : OneScriptableObject<OsoWithAsset>
    {
        public const string ResourcesPath = "one-asset-tests/OsoWithAsset";
    }
}