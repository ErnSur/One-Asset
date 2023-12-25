namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Assets/Resources/" + ResourcesPath + ".asset", CreateAssetIfMissing = true)]
    [SettingsProviderAsset(SettingsWindowPath)]
    internal class SettingsAssetAsSingleton : OneScriptableObject<OsoWithAsset>
    {
        public const string ResourcesPath = "one-asset-tests/SettingsAssetAsSingleton";
        public const string SettingsWindowPath = "Project/" + nameof(SettingsAssetAsSingleton);
    }
}