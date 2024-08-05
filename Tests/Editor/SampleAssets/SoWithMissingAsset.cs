using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("non/exising/path", AssetIsMandatory = true)]
    internal class SoWithMissingAsset : ScriptableObject
    {
    }
}