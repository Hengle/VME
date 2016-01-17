using UnityEngine;
using System.Collections;
using UnityEditor;

public class VMETileAddControls {

    /// <summary>
    /// The amount of tiles a tile group is a;lowed inside the scene.
    /// </summary>
    private const int AMOUNT_OF_TILES_PER_GROUP = 40;

    /// <summary>
    /// Edits a tile.
    /// </summary>
    /// <param name="_tileToCreate">The tile that you want to create at selected position.</param>
    /// <param name="_tileToRemove">The tile that you want to remove.</param>
    public void EditTile (GameObject _tileToCreate, GameObject _tileToRemove) {

        //Create the new tile.
        GameObject newTile = (GameObject)Object.Instantiate(_tileToCreate, _tileToRemove.transform.position, _tileToRemove.transform.rotation);
        newTile.name = _tileToCreate.name;

        //Split its name so we wont check a tile group for its index.
        string[] names = _tileToCreate.name.Split('_');

        //Check if theres a tile group available, create one if not found.
        GameObject parent = CheckForTileGroup(_tileToRemove.transform.parent.parent.gameObject, names[0], 0);

        //perform final tasks to the new tile.
        newTile.transform.parent = parent.transform;
        newTile.AddComponent<BoxCollider>();
        newTile.AddComponent<ChunkObjectData>();
        Selection.activeGameObject = newTile;

        //Destroy the tile you had selected.
        Object.DestroyImmediate(_tileToRemove);

    }

    public void PaintTIle(GameObject _tileToCreate,GameObject _hoveredTile,Vector3 _position) {

        //Create the new tile.
        GameObject newTile = (GameObject)Object.Instantiate(_tileToCreate, _hoveredTile.transform.position, _hoveredTile.transform.rotation);
        newTile.name = _tileToCreate.name;
        newTile.transform.position = _position;
        //Split its name so we wont check a tile group for its index.
        string[] names = _tileToCreate.name.Split('_');

        //Check if theres a tile group available, create one if not found.
        GameObject parent = CheckForTileGroup(_hoveredTile.transform.parent.parent.gameObject, names[0], 0);

        //perform final tasks to the new tile.
        newTile.transform.parent = parent.transform;
        newTile.AddComponent<BoxCollider>();
        newTile.AddComponent<ChunkObjectData>();
        //Selection.activeGameObject = newTile;

    }

    /// <summary>
    /// Check if theres a group inside the layer with the name of the tile and corresponding index.
    /// </summary>
    /// <param name="_layer">the layer thats used for this tile.</param>
    /// <param name="_name">the name of the tile.</param>
    /// <param name="_index">current index being checked.</param>
    /// <returns>The tile group.</returns>
    private GameObject CheckForTileGroup (GameObject _layer, string _name, int _index) {

        //try to find the layer object
        Transform targetLayer = _layer.transform.FindChild(_name + _index);

        //if it isn't found, create one.
        if (targetLayer == null) {

            GameObject obj = new GameObject();
            obj.name = (_name + _index);
            obj.transform.parent = _layer.transform;
            return obj;

        } else {

            //If the layer has more tiles than allowed.
            if (targetLayer.childCount >= AMOUNT_OF_TILES_PER_GROUP) {

                //perform same function with higher index number.
                return CheckForTileGroup(_layer, _name, _index + 1);

            } else {

                return targetLayer.gameObject;

            }

        }

    }

}
