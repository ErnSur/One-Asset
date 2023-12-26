using System.IO;
using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace QuickEye.OneAsset.Editor.Tests
{
    using static TestUtils;
    [TestOf(typeof(OneAssetLoader))]
    [InitializeOnLoad]
    public class AutomaticAssetCreationTests
    {
        private static bool _initializeOnLoadTestsPassed;
        static AutomaticAssetCreationTests()
        {
            RunInitializeOnLoadTests();
        }

        private static void RunInitializeOnLoadTests()
        {
            try
            {
                var tests = new AutomaticAssetCreationTests();
                tests.Setup();
                tests.Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing();
                tests.Teardown();
                tests.OneTimeTearDown();
                _initializeOnLoadTestsPassed = true;
            }
            catch (AssertionException)
            {
            }
        }

        [SetUp]
        public void Setup()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [TearDown]
        public void Teardown()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AssetDatabase.Refresh();
        }

        [Test]
        public void InitializeOnLoadTests()
        {
            Assert.IsTrue(_initializeOnLoadTestsPassed);
        }

        [Test]
        public void Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            var asset = OneAssetLoader.Load<SoWithCreateAutomatically>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically.AbsoluteAssetPath, assetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_AtPathFromTheAttributeWithHighestPriority()
        {
            var asset = OneAssetLoader.Load<SoWithCreateAutomatically2>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically2.AbsoluteAssetPathNoExt, assetPath);
            FileAssert.DoesNotExist(SoWithCreateAutomatically2.SecondaryAbsoluteAssetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_PathHasNoFileExtension()
        {
            var options = new AssetLoadOptions($"{TempDir}Resources/test")
            {
                CreateAssetIfMissing = true
            };
           
            var asset = OneAssetLoader.Load(options, typeof(SoWithAsset));

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(options.Paths[0], assetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_PathHasFileExtension()
        {
            var options = new AssetLoadOptions($"{TempDir}Resources/test.asset")
            {
                CreateAssetIfMissing = true
            };
            var asset = OneAssetLoader.Load(options, typeof(SoWithAsset));

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(options.Paths[0], assetPath);
        }

        private static void DeleteTestOnlyAssetsIfTheyExist()
        {
            if (Directory.Exists(TempDir))
            {
                AssetDatabase.DeleteAsset(TempDir);
            }
        }
    }
}