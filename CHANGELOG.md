# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [4.0.2] - 2023-08-26

### Fixed
- Added missing icon files


## [4.0.1] - 2023-08-16
 
### Added
- License and Changelog
 
## [4.0.0] - 2023-08-15

First standalone release of One Asset. This package was initially embedded in the [QuickEye-Utility](https://github.com/ErnSur/QuickEye-Utility) repository.
Big API refactor, OneAssetLoader became the source of the package functionality.

### Added
- Added support for "unsafe loading" with `AssetLoadOptions.LoadAndForget`. This allows for asset loading before `AssetDatabase` initialization

### Changed
- `SingletonAssetAttribute` -> `LoadFromAssetAttribute`
- `CreateAssetAutomaticallyAttribute` -> `LoadFromAssetAttribute.CreateAssetIfMissing`
- `SingletonMonoBehaviour` -> `OneGameObject`
- `SingletonScriptableObject` -> `OneScriptableObject`
- `ScriptableObjectFactory` -> `OneAssetLoader`