using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Assets/Resources/" + ResourcesPath + ".asset", CreateAssetIfMissing = true)]
    [SettingsProviderAsset(SettingsWindowPath)]
    internal class SettingsAsset : ScriptableObject
    {
        public const string ResourcesPath = "one-asset-tests/SettingsAsset";
        public const string SettingsWindowPath = "Project/" + nameof(SettingsAsset);
    }
}