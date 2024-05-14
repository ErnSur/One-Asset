using System.IO;

namespace QuickEye.OneAsset
{
    internal class AssetPath
    {
        public string OriginalPath { get; }
        public bool IsInResourcesFolder { get; }
        public string ResourcesPath { get; }
        public string Extension { get; }
        
        /// <param name="path">Path that starts from unity project root folder or "Resources" folder. Has to end with file extension.</param>
        public AssetPath(string path)
        {
            OriginalPath = path;
            Extension = Path.GetExtension(path);
            if (PathUtility.ContainsFolder("Resources", path))
            {
                IsInResourcesFolder = true;
                ResourcesPath = PathUtility.GetResourcesPath(path);
            }
        }

        public override string ToString()
        {
            return IsInResourcesFolder ? $"*/Resources/{ResourcesPath}" : OriginalPath;
        }
    }
}