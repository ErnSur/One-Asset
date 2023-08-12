using System.IO;
using System.Linq;
using UnityEditor;

namespace Editor
{
    public static class RegenerateGuidsAction
    {
        [MenuItem("Assets/Regenerate GUIDs")]
        public static void RegenerateGuids()
        {
            var selection = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToArray().First();

            if (!EditorUtility.DisplayDialog("GUIDs regeneration",
                    $"You are going to start the process of GUID regeneration.\n{selection}. \n\nMAKE A PROJECT BACKUP BEFORE PROCEEDING!",
                    "Regenerate GUIDs", "Cancel"))
                return;
            try
            {
                AssetDatabase.StartAssetEditing();

                var path = Path.GetFullPath(selection);
                var regenerator = new GuidRegenerator(path);
                regenerator.RegenerateGuids();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
            }
        }
    }
}