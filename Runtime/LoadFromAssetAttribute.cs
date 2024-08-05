using System;
using System.Text;

namespace QuickEye.OneAsset
{
    /// <summary>
    /// Applies loading rules for <see cref="OneAssetLoader"/>
    /// Can be used on <see cref="UnityEngine.ScriptableObject"/> and <see cref="UnityEngine.MonoBehaviour"/>
    /// Use multiple <see cref="LoadFromAssetAttribute"/> to look for the asset in multiple different paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class LoadFromAssetAttribute : Attribute
    {
        /// <summary>
        /// If Path starts with "Resources/" it will be loaded from resources and be available in runtime
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Enables a system that will create scriptable object file if it cannot be loaded from <see cref="LoadFromAssetAttribute.Path"/>
        /// </summary>        
        public bool CreateAssetIfMissing { get; set; }

        /// <summary>
        /// <para>If set to true a <see cref="AssetIsMissingException"/> will be thrown when <see cref="OneAssetLoader"/> will not find asset at any of the paths.</para>
        /// <para>By default: true</para>
        /// </summary>
        public bool AssetIsMandatory { get; set; } = true;

        /// <summary>
        /// Relevant for types with multiple <see cref="LoadFromAssetAttribute"/>.
        /// Optional field to specify the order in which asset is searched for. Paths with higher priority are searched first
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// In Editor, use the <see cref="UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget"/> as a fallback load option. Use with caution!
        /// </summary>
        public bool LoadAndForget { get; set; }

        /// <summary>
        /// Defines a path at which asset can be found for <see cref="OneAssetLoader"/>
        /// Valid on types like <see cref="UnityEngine.ScriptableObject"/> or <see cref="UnityEngine.MonoBehaviour"/>
        /// </summary>
        /// <param name="path">
        /// <para>Path from which <see cref="OneAssetLoader"/> will try to load an asset.</para>
        /// <para>If path is absolute and contains a file extension, it will work with all of the options.</para>
        /// <para>If path starts or contains the "/Resources/" it will be loaded using <see cref="UnityEngine.Resources"/> and be available in runtime.</para>
        /// </param>
        public LoadFromAssetAttribute(string path)
        {
            Path = path;
        }
    }
}