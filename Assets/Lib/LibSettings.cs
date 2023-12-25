using System.Collections;
using System.Collections.Generic;
using QuickEye.OneAsset;
using UnityEngine;

[LoadFromAsset("Assets/Test/LibSettings.asset", CreateAssetIfMissing = true)]
public class LibSettings : LibOneSo<LibSettings>
{
    public int age;
}

/// <summary>
/// Examples of how to use OneScriptableObject<T> without having user lib to reference OneAsset Asmdef
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LibOneSo<T> : OneScriptableObject<T> where T : LibOneSo<T>
{
    public new static LibSettings Instance => OneScriptableObject<LibSettings>.Instance; 
}