using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests
{
    using static TestUtils;

    [TestOf(typeof(OneAssetLoader))]
    public class OneAssetLoaderTests
    {
        private SoWithAsset _assetFromResourcesFolder;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _assetFromResourcesFolder = Resources.Load<SoWithAsset>(SoWithAsset.ResourcesPath);
            Assert.NotNull(_assetFromResourcesFolder);
        }

        [SetUp]
        public void SetUp()
        {
            DeleteTestOnlyDirectory();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTestOnlyDirectory();
        }

        [TestCase(TempDir + "Resources/" + UniqueFileName)]
        [TestCase("/" + TempDir + "Resources/" + UniqueFileName)]
        [TestCase(TempDir + "Resources/" + UniqueFileName + ".asset")]
        [TestCase(TempDir + "Resources/" + UniqueFileName + ".userExtension")]
        [TestCase(TempDir + "Resources/" + UniqueFileName + ".userExtension.asset")]
        public void Should_LoadAsset_When_AssetExistsInResources(string path)
        {
            var options = new AssetLoadOptions(path);
            using (var testAssetScope = new TestAssetScope(options.Paths[0]))
            {
                var actual = OneAssetLoader.Load(options, typeof(SoWithAsset));

                Assert.AreEqual(testAssetScope.Asset, actual);
            }
        }

        [TestCase(TempDir + UniqueFileName + ".asset")]
        [TestCase("/" + TempDir + UniqueFileName + ".asset")]
        [TestCase(TempDir + UniqueFileName + ".userExtension.asset")]
        public void Should_LoadAsset_When_AssetInNotInResources(string path)
        {
            var options = new AssetLoadOptions(path);
            using (var testAssetScope = new TestAssetScope(options.Paths[0]))
            {
                var actual = OneAssetLoader.Load(options, typeof(SoWithAsset));

                Assert.AreEqual(testAssetScope.Asset, actual);
            }
        }

        /// <summary>
        /// Mandatory asset is defined by <see cref="LoadFromAssetAttribute.AssetIsMandatory"/>
        /// </summary>
        [Test]
        public void Should_Throw_When_TypeHasMandatoryAssetButAssetIsMissing()
        {
            Assert.Throws<AssetIsMissingException>(() => { OneAssetLoader.Load<SoWithMissingAsset>(); });
        }

        [Test]
        public void Should_ReturnNull_When_TypeHasNonMandatoryAssetAndAssetIsMissing()
        {
            var result = OneAssetLoader.Load<SoWithNonMandatoryMissingAsset>();

            Assert.Null(result);
        }
    }
}