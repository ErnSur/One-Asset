using NUnit.Framework;
using QuickEye.OneAsset.Tests.SampleAssets;
using UnityEngine;

namespace QuickEye.OneAsset.Tests
{
    [TestOf(typeof(OneGameObject<>))]
    public class OneGameObjectTests
    {
        private GameObjectWithoutPrefab _instance;

        [SetUp]
        public void SetUp()
        {
            _instance = GameObjectWithoutPrefab.Instance;
        }

        [TearDown]
        public void TearDown()
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
            var actual = GameObjectWithoutPrefab.Instance;
            Assert.AreEqual(_instance, actual);
        }

        [Test]
        public void Should_CreateNewInstance_When_ExistingOneWasDestroyed()
        {
            Object.DestroyImmediate(_instance);

            Assert.NotNull(GameObjectWithoutPrefab.Instance);
        }
        
        [Test]
        public void Should_CreateNewInstanceFromPrefab_When_HasLoadFromAssetAttribute()
        {
            // Arrange
            var prefab = Resources.Load<GameObjectWithPrefab>(GameObjectWithPrefab.ResourcesPath);
            Assert.NotNull(prefab);

            // Act
            var instance = GameObjectWithPrefab.Instance;
            
            // Assert
            Assert.AreNotEqual(prefab, instance);
            GameObjectAssert.IsPrefabInstance(instance.gameObject);
        }
    }
}