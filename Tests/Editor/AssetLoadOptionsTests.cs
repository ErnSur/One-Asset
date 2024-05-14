using NUnit.Framework;

namespace QuickEye.OneAsset.Editor.Tests
{
    [TestOf(typeof(AssetLoadOptions))]
    public class AssetLoadOptionsTests
    {
        [TestCase("Assets/Resources/Test.asset", "Resources/Test.asset", ExpectedResult = true)]
        [TestCase("Assets/Test.asset", "Resources/Test.asset", ExpectedResult = false)]
        public bool Should_GetLoadPath(string fullAssetPath, string loadPath)
        {
            var loadOptions = new AssetLoadOptions(loadPath);
            return loadOptions.TryGetLoadAssetPath(fullAssetPath, out _);
        }
    }
}