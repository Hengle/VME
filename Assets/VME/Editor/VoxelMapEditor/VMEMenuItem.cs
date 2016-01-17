using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Used for adding VME related Menu Items.
/// </summary>
public class VMEMenuItem : MonoBehaviour {

    [MenuItem("VME/Create/VoxelMap")]
    static void CreateVoxelMap () {

        GameObject newVoxelMap = Instantiate(Resources.Load("VME_Data/DefaultVoxelMap")) as GameObject;
        newVoxelMap.name = "New VoxelMap";
        Selection.activeGameObject = newVoxelMap;
        SceneView.lastActiveSceneView.FrameSelected();

    }
	
}
