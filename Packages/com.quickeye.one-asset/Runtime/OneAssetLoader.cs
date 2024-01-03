using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.OneAsset
{
    using static AssetLoadOptionsUtility;

    // API Ideas:
    // 1. `public event Action<Object> AssetCreated;`
    // 2. ability to create assets using `UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget`
    // 3. Pass custom asset creation logic in `AssetLoadOptions`
    /// <summary>
    /// <para>Loads assets with <see cref="AssetLoadOptions"/></para>
    /// </summary>
    public static class OneAssetLoader
    {
        private static readonly IOneAssetLoaderEditorFeatures EditorFeatures;

        static OneAssetLoader()
        {
            if (Application.isEditor)
                EditorFeatures = new OneAssetLoaderEditorFeatures();
        }

        /// <summary>
        /// <para>Loads an asset with load options</para>
        /// </summary>
        /// <param name="options">Options defining the behaviour of load operation</param>
        /// <param name="assetType">Type of the asset</param>
        /// <returns>Asset instance or null if asset is missing</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and asset creation failed</exception>
        public static Object Load(AssetLoadOptions options, Type assetType)
        {
            if (options == null || options.Paths.Length == 0)
                return null;

            // Try to load asset
            if (TryLoad(assetType, options, out var asset))
                return asset;

            // Try to create asset at path
            if (options.CreateAssetIfMissing &&
                typeof(ScriptableObject).IsAssignableFrom(assetType) &&
                TryCreateAsset(assetType, options) &&
                TryLoad(assetType, options, out asset))
                return asset;

            // Throw if asset is mandatory
            if (options.AssetIsMandatory)
                throw new AssetIsMissingException(assetType, options.Paths[0]);

            return null;
        }

        /// <summary>
        /// <para>Loads an asset with load options</para>
        /// </summary>
        /// <typeparam name="T">Type of the asset</typeparam>
        /// <param name="options">Options defining the behaviour of load operation</param>
        /// <returns>Asset instance or null if asset is missing</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and asset creation failed</exception>
        public static T Load<T>(AssetLoadOptions options) where T : Object
        {
            return Load(options, typeof(T)) as T;
        }

        /// <summary>
        /// <para>Loads an asset with load options</para>
        /// <para>The <see cref="AssetLoadOptions"/> will be created based on the <see cref="LoadFromAssetAttribute"/> from asset type</para>
        /// </summary>
        /// <returns>Asset instance or null if asset is missing</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and asset creation failed</exception>
        public static Object Load(Type assetType)
        {
            return Load(GetLoadOptions(assetType), assetType);
        }

        /// <summary>
        /// <para>Loads an asset with load options</para>
        /// <para>The <see cref="AssetLoadOptions"/> will be created based on the <see cref="LoadFromAssetAttribute"/> from asset type</para>
        /// </summary>
        /// <typeparam name="T">Type of the asset</typeparam>
        /// <returns>Asset instance or null if asset is missing</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and asset creation failed</exception>
        public static T Load<T>() where T : Object
        {
            return Load(typeof(T)) as T;
        }

        private static bool TryCreateAsset(Type type, AssetLoadOptions options)
        {
            if (!Application.isEditor)
                return false;
            if (EditorFeatures == null)
                throw new NotImplementedException(
                    "Asset trying to be created in editor, but editor features are missing.");
            var obj = ScriptableObject.CreateInstance(type);
            try
            {
                EditorFeatures.CreateAsset(obj, options);
                return true;
            }
            catch (Exception e)
            {
                Object.DestroyImmediate(obj);
                throw new EditorAssetFactoryException(type, e);
            }
        }

        private static bool TryLoad(Type type, AssetLoadOptions options, out Object obj)
        {
            foreach (var path in options.AssetPaths)
            {
                if (TryLoadFromResources(type, path, out obj))
                    return true;

                if (EditorFeatures?.TryLoadFromAssetDatabase(type, path, out obj) == true)
                    return true;

                if (options.LoadAndForget && EditorFeatures?.TryLoadAndForget(type, path, out obj) == true)
                    return true;
            }

            obj = null;
            return false;
        }

        private static bool TryLoadFromResources(Type type, AssetPath path, out Object obj)
        {
            if (path.IsInResourcesFolder)
            {
                obj = Resources.Load(path.ResourcesPath, type);
                if (obj != null)
                    return true;
            }

            obj = null;
            return false;
        }
    }
}