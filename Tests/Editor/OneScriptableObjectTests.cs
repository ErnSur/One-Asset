using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests
{
    [TestOf(typeof(OneScriptableObject<>))]
    public class OneScriptableObjectTests
    {
        private OsoWithNoAsset _instance;

        [SetUp]
        public void Setup()
        {
            _instance = OsoWithNoAsset.Instance;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_instance);
        }

        [Test]
        public void Should_CreateNewInstance_When_NoInstanceExists()
        {
            Assert.NotNull(_instance);
        }

        [Test]
        public void Should_ReturnSameInstance_When_OneInstanceExists()
        {
            var actual = OsoWithNoAsset.Instance;
            Assert.AreEqual(_instance, actual);
        }

        [Test]
        public void Should_CreateNewInstance_When_ExistingOneWasDestroyed()
        {
            Object.DestroyImmediate(_instance);

            Assert.NotNull(OsoWithNoAsset.Instance);
        }

        [Test]
        public void Should_ReturnAsset_When_HasLoadFromAttribute()
        {
            var asset = Resources.Load<OsoWithAsset>(OsoWithAsset.ResourcesPath);
            Assert.NotNull(asset);

            var actual = OsoWithAsset.Instance;
            
            Assert.AreEqual(asset, actual);
        }
    }
}