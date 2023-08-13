using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEye.OneAsset.Editor.UI
{
    internal static class LoadableAssetGUI
    {
        private static readonly string LinkedIcon;
        private static readonly string UnlinkedIcon;
        
        static LoadableAssetGUI()
        {
            LinkedIcon = GetIconPath("OneAssetLinked","Linked");
            UnlinkedIcon = GetIconPath("OneAssetUnLinked","Unlinked");
        }

        public static GUIContent GetGuiContent(bool isCorrectPath, string loadPath, string typeName)
        {
            var iconContent = isCorrectPath
                ? EditorGUIUtility.IconContent(LinkedIcon)
                : EditorGUIUtility.IconContent(UnlinkedIcon);
            iconContent.tooltip = isCorrectPath
                ? $"{typeName} can be loaded from this path"
                : $"{typeName} won't load from this path. Load path:\n\"{loadPath}\"";
            return iconContent;
        }

        private static string GetIconPath(string iconLabel, string fallbackPath)
        {
            try
            {
                var path = AssetDatabase
                    .FindAssets($"l:{iconLabel}")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .First();
                return path;
            }
            catch (Exception)
            {
                return fallbackPath;
            }
        }
    }
}