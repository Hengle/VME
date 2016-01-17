using UnityEngine;
using UnityEditor;

public class AssetCreation {

    [MenuItem("Assets/Create/VME/Swatch")]
    public static void CreateTowerDataAsset () {
        ScriptableObjectUtility.CreateAsset<VoxelSwatch>();
    }

    [MenuItem("Assets/Create/VME/Settings Values")]
    public static void CreateSettingsAsset () {
        Debug.LogWarning("WARNING: Only one Settings value is needed. this menu item is for accidental removal only. The location of this asset should be at " +
            "[VME/Data] with the name [VMESettings].");
        ScriptableObjectUtility.CreateAsset<VMESettingsObject>();
    }

}