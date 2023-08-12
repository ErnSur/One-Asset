using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(AbsoluteAssetPath, CreateAssetIfMissing = true)]
    internal class SoWithCreateAutomatically : ScriptableObject
    {
        public const string AbsoluteAssetPath =
            TestUtils.TempDir
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically);
    }
}