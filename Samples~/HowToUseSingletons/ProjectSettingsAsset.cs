﻿using UnityEngine;

namespace QuickEye.OneAsset.HowToUseSingletons
{
    /// <summary>
    /// Creates a new Project Settings Window page using <see cref="OneScriptableObject"/>
    /// with <see cref="LoadFromAssetAttribute"/> and <see cref="SettingsProviderAssetAttribute"/>
    /// </summary>
    [LoadFromAsset(AssetPath, AssetIsMandatory = true, CreateAssetIfMissing = true)]
    [SettingsProviderAsset("Project/" + SettingsPageName)]
    public class ProjectSettingsAsset : OneScriptableObject<ProjectSettingsAsset>
    {
        private const string AssetPath = "Assets/Samples/Settings/Resources/" + nameof(ProjectSettingsAsset);
        private const string SettingsPageName = "Your Settings Page Name";

        [TextArea]
        public string Description =
            $"To see new Project Window Settings Page open menu Edit/Project Settings. And find a page named: {SettingsPageName}";
    }
}