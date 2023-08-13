using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests.SampleAssets
{
    internal class SoWithInheritedCreateAutomatically : SoWithInheritedCreateAutomaticallyBase
    {
    }

    [LoadFromAsset("Resources/"+ResourcesDirectory, CreateAssetIfMissing = true)]
    internal abstract class SoWithInheritedCreateAutomaticallyBase : ScriptableObject
    {
        private const string ResourcesDirectory = "one-asset-tests/";
    }
}