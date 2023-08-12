using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Editor
{
    internal class GuidRegenerator
    {
        private static readonly string[] DefaultFileExtensions =
        {
            "*.meta",
            "*.mat",
            "*.anim",
            "*.prefab",
            "*.unity",
            "*.asset",
            "*.guiskin",
            "*.fontsettings",
            "*.controller",
        };

        private readonly string _assetsPath;

        public GuidRegenerator(string assetsPath)
        {
            _assetsPath = assetsPath;
        }

        public void RegenerateGuids(string[] regeneratedExtensions = null)
        {
            if (regeneratedExtensions == null)
            {
                regeneratedExtensions = DefaultFileExtensions;
            }

            // Get list of working files
            var filesPaths = new List<string>();
            foreach (var extension in regeneratedExtensions)
            {
                filesPaths.AddRange(
                    Directory.GetFiles(_assetsPath, extension, SearchOption.AllDirectories)
                );
            }

            // Create dictionary to hold old-to-new GUID map
            var guidOldToNewMap = new Dictionary<string, string>();
            var guidsInFileMap = new Dictionary<string, List<string>>();

            // We must only replace GUIDs for Resources present in Assets. 
            // Otherwise built-in resources (shader, meshes etc) get overwritten.
            var ownGuids = new HashSet<string>();

            // Traverse all files, remember which GUIDs are in which files and generate new GUIDs
            var counter = 0;
            foreach (var filePath in filesPaths)
            {
                if (!EditorUtility.DisplayCancelableProgressBar("Scanning Assets folder",
                        MakeRelativePath(_assetsPath, filePath),
                        counter / (float)filesPaths.Count))
                {
                    var contents = File.ReadAllText(filePath);

                    var guids = GetGuids(contents);
                    var isFirstGuid = true;
                    foreach (var oldGuid in guids)
                    {
                        // First GUID in .meta file is always the GUID of the asset itself
                        if (isFirstGuid && Path.GetExtension(filePath) == ".meta")
                        {
                            ownGuids.Add(oldGuid);
                            isFirstGuid = false;
                        }

                        // Generate and save new GUID if we haven't added it before
                        if (!guidOldToNewMap.ContainsKey(oldGuid))
                        {
                            var newGuid = Guid.NewGuid().ToString("N");
                            guidOldToNewMap.Add(oldGuid, newGuid);
                        }

                        if (!guidsInFileMap.ContainsKey(filePath))
                            guidsInFileMap[filePath] = new List<string>();

                        if (!guidsInFileMap[filePath].Contains(oldGuid))
                        {
                            guidsInFileMap[filePath].Add(oldGuid);
                        }
                    }

                    counter++;
                }
                else
                {
                    UnityEngine.Debug.LogWarning("GUID regeneration canceled");
                    return;
                }
            }

            // Traverse the files again and replace the old GUIDs
            counter = -1;
            var guidsInFileMapKeysCount = guidsInFileMap.Keys.Count;
            foreach (var filePath in guidsInFileMap.Keys)
            {
                EditorUtility.DisplayProgressBar("Regenerating GUIDs", MakeRelativePath(_assetsPath, filePath),
                    counter / (float)guidsInFileMapKeysCount);
                counter++;

                var contents = File.ReadAllText(filePath);
                foreach (var oldGuid in guidsInFileMap[filePath])
                {
                    if (!ownGuids.Contains(oldGuid))
                        continue;

                    var newGuid = guidOldToNewMap[oldGuid];
                    if (string.IsNullOrEmpty(newGuid))
                        throw new NullReferenceException("newGuid == null");

                    contents = contents.Replace("guid: " + oldGuid, "guid: " + newGuid);
                }

                File.WriteAllText(filePath, contents);
            }

            EditorUtility.ClearProgressBar();
        }

        private static IEnumerable<string> GetGuids(string text)
        {
            const string guidStart = "guid: ";
            const int guidLength = 32;
            var textLength = text.Length;
            var guidStartLength = guidStart.Length;
            var guids = new List<string>();

            var index = 0;
            while (index + guidStartLength + guidLength < textLength)
            {
                index = text.IndexOf(guidStart, index, StringComparison.Ordinal);
                if (index == -1)
                    break;

                index += guidStartLength;
                var guid = text.Substring(index, guidLength);
                index += guidLength;

                if (IsGuid(guid))
                {
                    guids.Add(guid);
                }
            }

            return guids;
        }

        private static bool IsGuid(string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (
                    !((c >= '0' && c <= '9') ||
                      (c >= 'a' && c <= 'z'))
                )
                    return false;
            }

            return true;
        }

        private static string MakeRelativePath(string fromPath, string toPath)
        {
            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath;
        }
    }
}