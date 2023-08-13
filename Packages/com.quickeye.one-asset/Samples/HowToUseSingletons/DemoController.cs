using System;
using UnityEngine;

namespace QuickEye.OneAsset.HowToUseSingletons
{
    public class DemoController : MonoBehaviour
    {
        [SerializeField]
        private TypeWithoutAnAsset typeWithoutAnAsset;
        
        [SerializeField]
        private TypeWithOptionalAsset typeWithOptionalAsset;
        
        [SerializeField]
        private TypeWithMandatoryAsset typeWithMandatoryAsset;
        
        [SerializeField]
        private ProjectSettingsAsset projectSettingsAsset;
        
        private void Awake()
        {
            // Example usage of different types of singletons:
            ExampleUsageOfOneScriptableObject();
        }

        private void ExampleUsageOfOneScriptableObject()
        {
            // Regular singleton behaviour:
            // Create and share one instance
            typeWithoutAnAsset = TypeWithoutAnAsset.Instance;

            // Same as above 
            // but if there is an asset at TypeWithOptionalAsset.ResourcesPath
            // the instance will be loaded from it
            typeWithOptionalAsset = TypeWithOptionalAsset.Instance;

            // Same as above
            // but if no asset is found the `Instance` will throw an exception
            // also the `LoadFromAsset.CreateAssetIfMissing` is set to true
            // this will create asset if it is not found before throwing the exception
            typeWithMandatoryAsset = TypeWithMandatoryAsset.Instance;

            // Same as above
            // but the `SettingsProviderAssetAttribute`
            // allows the editor to edit this asset in ProjectSettings
            projectSettingsAsset = ProjectSettingsAsset.Instance;
        }

        private void Update()
        {
            // PopupView is a GameObject Singleton
            PopupView.Instance.SetMessage($"Hello World!\n{DateTime.Now.TimeOfDay:hh\\:mm\\:ss}");
        }
    }
}