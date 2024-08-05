using System.IO;
using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace QuickEye.OneAsset.Editor.Tests
{
    using System;
    using UnityEngine;
    using static TestUtils;

    [TestOf(typeof(OneAssetLoader))]
    [InitializeOnLoad]
    public class AutomaticAssetCreationTests
    {
        private static Exception _initializeOnLoadException;

        static AutomaticAssetCreationTests()
        {
            RunInitializeOnLoadTests();
        }

        private static void RunInitializeOnLoadTests()
        {
            var tests = new AutomaticAssetCreationTests();
            try
            {
                tests.Setup();
                tests.Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing();
            }
            catch (Exception e)
            {
                _initializeOnLoadException = e;
            }
            finally
            {
                tests.Teardown();
                tests.OneTimeTearDown();
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
            if (_initializeOnLoadException == null)
                return;
            Debug.LogException(_initializeOnLoadException);
            Assert.Fail("Failed");
        }

        [Test]
        public void Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            var asset = OneAssetLoader.Load<SoWithCreateAutomatically>();

            Assert.IsNotNull(asset);
            FileAssert.Exists(SoWithCreateAutomatically.AbsoluteAssetPathWithExtension);
            
            // Since Unity 6 asset database does not see assets created from InitializeOnLoad callback
            // https://issuetracker.unity3d.com/issues/resources-dot-load-fails-to-load-assets-created-in-the-same-frame-when-called-from-a-function-with-the-initializeonloadmethod-attribute
            // https://unity3d.atlassian.net/servicedesk/customer/portal/2/IN-77788
            // StringAssert.Contains(SoWithCreateAutomatically.AbsoluteAssetPathWithExtension, AssetDatabase.GetAssetPath(asset));
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