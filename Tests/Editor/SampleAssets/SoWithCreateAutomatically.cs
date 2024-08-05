using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(AbsoluteAssetPathWithExtension, CreateAssetIfMissing = true)]
    internal class SoWithCreateAutomatically : ScriptableObject
    {
        public const string AbsoluteAssetPathWithExtension =
            TestUtils.TempDir
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }
}