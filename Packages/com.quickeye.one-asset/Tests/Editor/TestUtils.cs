using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using QuickEye.OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

namespace QuickEye.OneAsset.Editor.Tests
{
    public static class TestUtils
    {
        public const string TempDir = "Assets/one-asset-tests/";
        public const string UniqueFileName = "2e18d746-214c-4557-8e9d-f8f15389f253";

        public static AssetLoadOptions CreateLoadOptionsWithUniquePath(params string[] pathWithoutFileName)
        {
            var paths = pathWithoutFileName.Select(p => $"{TempDir}{p}/{Guid.NewGuid().ToString()}").ToArray();
            return new AssetLoadOptions(paths);
        }
        public static ScriptableObject CreateTestSoAsset(string path)
        {
            path = PathUtility.EnsurePathStartsWith("Assets", path);
            if (!path.EndsWith(".asset"))
                path = $"{path}.asset";
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            var dirName = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);

            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return so;
        }

        public static void DeleteTestOnlyDirectory()
        {
            if (Directory.Exists(TempDir))
            {
                AssetDatabase.DeleteAsset(TempDir);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    internal class TestAssetScope : IDisposable
    {
        public ScriptableObject Asset { get; }

        public TestAssetScope(string path)
        {
            Asset = TestUtils.CreateTestSoAsset(path);
            //Debug.Log($"Created asset at: {AssetDatabase.GetAssetPath(Asset)}");
            Assert.NotNull(Asset);
            Assert.IsTrue(AssetDatabase.Contains(Asset));
        }

        public void Dispose()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Asset));
        }
    }
}