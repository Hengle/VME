using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Spawn chunk inside the editor, and save it.
/// </summary>
public class ChunkSpawner : MonoBehaviour {

    public Chunk chunk;
    private string resourcePath;
	
    void Update () {

    }

	public void Load () {

        chunk = Instantiate(Resources.Load(resourcePath, typeof(GameObject)),transform.position,Quaternion.identity) as Chunk;
        
    }

    public void Unload () {

        Destroy(chunk);

    }

    #if UNITY_EDITOR

    public void SaveInProject (string path) {

        if(chunk != null) {

            resourcePath = path + "/" + transform.name;
            PrefabUtility.CreatePrefab("Assets/VME/Resources/VME_Chunks/" + path + "/" + transform.name + ".prefab", chunk.gameObject);

        }

    }

    public void LoadFromProject(string path) {

        if(chunk != null) {

            Object.DestroyImmediate(chunk.gameObject);

        }
     
        if (!AssetDatabase.IsValidFolder("Assets/VME/Resources/VME_Chunks/" + path)) {
            
            AssetDatabase.CreateFolder("Assets/VME/Resources/VME_Chunks", path);

        }

        if (Resources.Load("VME_Chunks/" + path + "/" + transform.name, typeof(GameObject)) != null){

            GameObject chunkobj = Instantiate(Resources.Load("VME_Chunks/" + path + "/" + transform.name, typeof(GameObject))) as GameObject;
            Debug.Log("VME_Chunks/" + path + "/" + transform.name);
            chunk = chunkobj.GetComponent<Chunk>();
            chunk.name = chunkobj.name;
            Debug.Log("qwe");
        }

        if(chunk == null) {

            Debug.Log("Chunk not found at [" + path + "/" + transform.name + "]. Creating new one");
    
            GameObject obj = Instantiate(Resources.Load("VME_Data/DefaultChunk", typeof(GameObject))) as GameObject;
            obj.name = transform.name;
            PrefabUtility.CreatePrefab("Assets/VME/Resources/VME_Chunks/" + path + "/" + transform.name + ".prefab", obj);
            obj.transform.position = transform.position;
            obj.transform.parent = transform;

            chunk = obj.GetComponent<Chunk>();

        } else { 

           
            chunk.transform.position = transform.position;
            chunk.transform.parent = transform;

        }

    }

    #endif
}
