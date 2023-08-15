using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuickEye.OneAsset
{
    public class AssetLoadOptions
    {
        /// <summary>
        /// Paths from which <see cref="OneAssetLoader"/> will try to load an asset.
        /// Asset will be loaded from the first path that contains loadable asset.
        /// If path starts with "Resources/" it will be loaded from resources and be available in runtime.
        /// </summary>
        public string[] Paths { get; }

        internal AssetPath[] AssetPaths { get; private set; }

        /// <summary>
        /// <para>If set to true a <see cref="AssetIsMissingException"/> will be thrown when <see cref="OneAssetLoader"/> will not find asset at any of the paths.</para>
        /// <para>By default: true</para>
        /// </summary>
        public bool AssetIsMandatory { get; set; }

        /// <summary>
        /// In Editor, enables a system that will create scriptable object file if it cannot be loaded from <see cref="AssetLoadOptions.Paths"/>. It will always create asset at the first path from the <see cref="Paths"/> property.
        /// </summary>
        public bool CreateAssetIfMissing { get; set; }

        /// <summary>
        /// In Editor, use the <see cref="UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget"/> as a fallback load option. Use with caution!
        /// </summary>
        public bool LoadAndForget { get; set; }

        /// <param name="path">
        /// <para>Path from which <see cref="OneAssetLoader"/> will try to load an asset.</para>
        /// <para>If path is absolute and contains a file extension, it will work with all of the options.</para>
        /// <para>If path starts or contains the "/Resources/" it will be loaded using <see cref="UnityEngine.Resources"/> and be available in runtime.</para>
        /// </param>
        public AssetLoadOptions(string path) : this(new[] { path })
        {
        }

        /// <param name="paths">
        /// <para>Paths from which <see cref="OneAssetLoader"/> will try to load an asset.</para>
        /// <para>Asset will be loaded from the first path that contains loadable asset.</para>
        /// <para>If path is absolute and contains a file extension, it will work with all of the options.</para>
        /// <para>If path starts or contains the "/Resources/" it will be loaded using <see cref="UnityEngine.Resources"/> and be available in runtime.</para>
        /// </param>
        public AssetLoadOptions(IEnumerable<string> paths)
        {
            Paths = paths.Select(CleanPath).ToArray();
            AssetPaths = Paths.Select(p => new AssetPath(p)).ToArray();
        }

        // TODO: Do more cleaning to remove trailing slashes and so on. It will be best if the Path is as predictable as possible
        private static string CleanPath(string path)
        {
            return path.TrimStart('/');
        }
        
        internal bool TryGetLoadAssetPath(string fullAssetPath, out AssetPath loadPath)
        {
            if (string.IsNullOrEmpty(fullAssetPath))
            {
                loadPath = null;
                return false;
            }

            foreach (var loadablePath in AssetPaths)
            {
                if (IsPointingToTheSameResourcesFile(fullAssetPath, loadablePath))
                {
                    loadPath = loadablePath;
                    return true;
                }

                if (loadablePath.OriginalPath == fullAssetPath)
                {
                    loadPath = loadablePath;
                    return true;
                }
            }

            loadPath = null;
            return false;
        }

        private static bool IsPointingToTheSameResourcesFile(string fullAssetPath,
            AssetPath loadablePath)
        {
            return loadablePath.IsInResourcesFolder && fullAssetPath.EndsWith($"Resources/{loadablePath.ResourcesPath}{loadablePath.Extension}");
        }
    }
}