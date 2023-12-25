using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.OneAsset.Editor
{
    internal static class SettingsProviderFactory
    {
        [SettingsProviderGroup]
        private static SettingsProvider[] GetProviders()
        {
            var providers = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where Attribute.IsDefined(type, typeof(SettingsProviderAssetAttribute))
                where Attribute.IsDefined(type, typeof(LoadFromAssetAttribute))
                let settingsAssetAttribute = type.GetCustomAttribute<SettingsProviderAssetAttribute>()
                let provider = CreateSettingsProvider(settingsAssetAttribute.SettingsWindowPath, type)
                where provider != null
                select provider;

            return providers.ToArray();
        }

        private static SettingsProvider CreateSettingsProvider(string settingsWindowPath, Type type)
        {
            try
            {
                var asset = LoadAsset(type);
                var provider = AssetSettingsProvider.CreateProviderFromObject(
                    settingsWindowPath, asset,
                    SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(asset)));

                return provider;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create a settings provider for: {type.Name}, {e}");
                return null;
            }
        }

        private static ScriptableObject LoadAsset(Type type)
        {
            var loadOptions = AssetLoadOptionsUtility.GetLoadOptions(type);
            loadOptions.AssetIsMandatory = true;
            var asset = OneAssetLoader.Load(loadOptions, type) as ScriptableObject;
            return asset;
        }
    }
}