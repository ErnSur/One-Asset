using System.Linq;
using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuickEye.OneAsset.Editor.Tests
{
    using static TestUtils;

    [TestOf(typeof(SettingsProviderFactory))]
    public class SettingsProviderFactoryTests
    {
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

        [Test]
        public void Should_CreateProvider_When_ClassHasLoadFromAndSettingsProviderAttributes()
        {
            // Arrange
            const string settingsPath = SettingsAsset.SettingsWindowPath;
            var providers = SettingsProviderFactory.GetProviders();

            // Act
            var provider = providers.FirstOrDefault(x => x.settingsPath == settingsPath) as AssetSettingsProvider;

            // Assert
            Assert.IsNotNull(provider);
            provider.OnActivate(null,new VisualElement());
            var settingsAsset = OneAssetLoader.Load<SettingsAsset>();
            Assert.AreEqual(settingsAsset, provider.settingsEditor.target);
        }
        
        [Test]
        public void Should_LoadSingletonInstance_When_SettingsProviderIsCreatedFromOneScriptableObject()
        {
            // Arrange
            const string settingsPath = SettingsAssetAsSingleton.SettingsWindowPath;
            var providers = SettingsProviderFactory.GetProviders();

            // Act
            var provider = providers.FirstOrDefault(x => x.settingsPath == settingsPath) as AssetSettingsProvider;

            // Assert
            Assert.IsNotNull(provider);
            provider.OnActivate(null,new VisualElement());
            var settingsAsset = OneAssetLoader.Load<SettingsAssetAsSingleton>();
            Assert.AreEqual(settingsAsset, provider.settingsEditor.target);
        }
    }
}