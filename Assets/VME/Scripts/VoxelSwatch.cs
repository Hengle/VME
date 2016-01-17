using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;


[System.Serializable]
public class VoxelSwatch : ScriptableObject {

    public List<VSCategory> categories;
    public List<QuickReference> references;

#if UNITY_EDITOR

    [ContextMenu("Load content")]
    public void LoadSwatches () {


        if (categories == null) {

            categories = new List<VSCategory>();

        }

        if (references == null) {

            references = new List<QuickReference>();

        }

        categories.Clear();
        references.Clear();

        string Path = AssetDatabase.GetAssetPath(this).Replace('\\', '/').Replace(this.name + ".asset", "");
        string TilePath = Path + "SwatchFiles/Tiles";
        string PropsPath = Path + "SwatchFiles/Props";
        string EffectsPath = Path + "SwatchFiles/Effects";

        VSCategory TileCategory = new VSCategory("Tiles",TilePath,this);
        categories.Add(TileCategory);

        VSCategory PropsCategory = new VSCategory("Props",PropsPath,this);
        categories.Add(PropsCategory);

        VSCategory EffectCategory = new VSCategory("Effects",EffectsPath,this);
        categories.Add(EffectCategory);

        //Get all files
        string[] excistingPrefabGroups = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);


        foreach (string fbxFile in excistingPrefabGroups) {

            string assetPath = "Assets" + fbxFile.Replace(Application.dataPath, "").Replace('\\', '/');

            if (assetPath.Contains(TilePath)) {

                GameObject tileobject = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
                string[] names = tileobject.name.Split('_');

                TileCategory.AddItemGroup(tileobject, names[0], int.Parse(names[1]));

            }

        }
       
    }

    /// <summary>
    /// Finds the item with its Full name (ex: NormalTile_01)
    /// </summary>
    /// <param name="_itemName">The name of the tile group.</param>
    /// <returns>The Item group with the _itemName</returns>
    public VSTileGroup GetGroupByName (string _itemName) {

        for(int i = 0; i < references.Count; i++) {

            if(references[i].identifierName == _itemName) {

                return references[i].groupReference;

            }

        }

        return null;

    }

    /// <summary>
    /// Finds the item with its shortened name (ex: NormalTile)
    /// </summary>
    /// <param name="_itemName">The name of the tile group.</param>
    /// <returns>The Item group with the _itemName</returns>
    public VSTileGroup GetGroupByShortenedName (string _itemName) {

        for (int i = 0; i < references.Count; i++) {

            if (references[i].identifierName.Split('_')[0] == _itemName) {

                return references[i].groupReference;

            }

        }

        return null;

    }

#endif

}

[System.Serializable]
public class VSCategory {

    public VoxelSwatch voxelSwatchReference;

    public string categoryName;
    public string directoryPath;
    public List<VSTileGroup> tilegroups;

    public VSCategory (string _name,string _directoryPath,VoxelSwatch _voxelSwatchReference) {

        categoryName = _name;
        directoryPath = _directoryPath;
        tilegroups = new List<VSTileGroup>();
        voxelSwatchReference = _voxelSwatchReference;

    }

    public VSTileGroup AddItemGroup (GameObject _object, string _groupname, int _index) {

        CheckGroupRecursive(_groupname).AddStyle(_object);
        return CheckGroupRecursive(_groupname);

    }

    public void RemoveByName(string _name) {

        for (int i = 0; i < tilegroups.Count; i++) {

            if (tilegroups[i].groupName == _name) {

                tilegroups[i].RemoveAllStyles();
                tilegroups.Remove(tilegroups[i]);

            }

        }

    }

    public void CreateNewTileGroup (string _groupName) {

        //Create a new prefab at path
        GameObject newTileObject = new GameObject(_groupName + "_01");
        
        GameObject prefab = PrefabUtility.CreatePrefab(directoryPath + "/" + newTileObject.name + ".prefab", newTileObject);
        VSTileGroup group = AddItemGroup(newTileObject, _groupName, 0);
        voxelSwatchReference.references.Add(new QuickReference(_groupName + "_01", this, group));
        group.styles[0] = prefab;
        Object.DestroyImmediate(newTileObject);

    }

    public VSTileGroup CheckGroup (string _groupName) {

        for (int i = 0; i < tilegroups.Count; i++) {

            if (tilegroups[i].groupName == _groupName) {

                return tilegroups[i];

            }

        }

        return null;

    }

    VSTileGroup CheckGroupRecursive (string _groupName) {

        for (int i = 0; i < tilegroups.Count; i++) {

            if (tilegroups[i].groupName == _groupName) {

                return tilegroups[i];

            }

        }

        VSTileGroup group = new VSTileGroup(_groupName,this);
        tilegroups.Add(group);
        return group;

    }

}

[System.Serializable]
public class VSTileGroup {

    [System.NonSerialized]
    public VSCategory categoryReference;

    public string groupName;
    public List<GameObject> styles;

    public VSTileGroup (string _name, VSCategory _categoryReference) {

        groupName = _name;
        categoryReference = _categoryReference;
        styles = new List<GameObject>();

    }

    public void AddStyle (GameObject _object) {
        styles.Add(_object);
        categoryReference.voxelSwatchReference.references.Add(new QuickReference(_object.name,categoryReference, this));
    }

    public void RemoveStyle(GameObject _object) {
        
        AssetDatabase.DeleteAsset(categoryReference.directoryPath + "/" + _object.name + ".prefab");
        AssetDatabase.SaveAssets();

    }

    public void RemoveAllStyles () {

        for(int i = 0; i < styles.Count; i++) {

            RemoveStyle(styles[i]);

        }

    }

    public void AddNewStyle (string _styleName,int _index) {
        //Create a new prefab at path
        string indexName = "";
        if(_index < 10) {
            indexName = "_0" + _index;
        } else {
            indexName = "_" + _index;
        }

        GameObject newTileObject = new GameObject(_styleName + indexName);
        Debug.Log(newTileObject);
        Debug.Log(categoryReference);
        GameObject prefab = PrefabUtility.CreatePrefab(categoryReference.directoryPath + "/" + newTileObject.name + ".prefab", newTileObject);
        categoryReference.voxelSwatchReference.references.Add(new QuickReference(_styleName + "_01", categoryReference, this));
        styles.Add(prefab);

        Object.DestroyImmediate(newTileObject);

    }
    
}

/// <summary>
/// Used for looking up a item group by its name.
/// </summary>
[System.Serializable]
public class QuickReference {

    public QuickReference (string _identifier,VSCategory _categoryReference,VSTileGroup _group) {
        groupReference = _group;
        identifierName = _identifier;
        categoryReference = _categoryReference;
    }

    public VSTileGroup groupReference;
    public VSCategory categoryReference;
    public string identifierName;

}