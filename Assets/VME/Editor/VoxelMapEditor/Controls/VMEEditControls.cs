using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Threading;

namespace VME {

    /// <summary>
    /// Handles all related actions of editing a tile.
    /// actions like: 
    /// 
    /// [edit tile to selection in TileSwatch. (single or multiple selection) ]
    /// [Change style of hovered tile]
    /// [Rotate hovered tile]
    /// </summary>
    public class VMEEditControls {

        public VMEEditControls (VMEVoxelSwatchPanel _voxelswatchpanel) {

            voxelSwatchReference = _voxelswatchpanel;
            settingsObject = VMESettingsObject.LoadScriptableObject();

        }

        /// <summary>
        /// Reference to the Object that holds the editor settings.
        /// </summary>
        private VMESettingsObject settingsObject;

        /// <summary>
        /// Reference to the voxel swatch Panel.
        /// </summary>
        private VMEVoxelSwatchPanel voxelSwatchReference;

        /// <summary>
        /// Handles input for the editor.
        /// </summary>
        public void Input (SceneView view) {

            Event e = Event.current;

            if (e.isKey) {

                if (e.type == EventType.KeyDown) {

                    if (e.keyCode == settingsObject.CHANGEBLOCK_STYLE) {

                        TryToSwitchTileStyle();

                    }

                    if (e.keyCode == settingsObject.APPLY_SINGLE) {

                        EditSingleTarget();

                    }

                    if (e.keyCode == settingsObject.APPLY_ALL) {

                        EditMultipleTargets();

                    }

                    if (e.keyCode == settingsObject.ROTATE_TARGET) {

                        TryToRotateTile(90);

                    }

                }

            }

        }

        /// <summary>
        /// Change selected chunkObject to selected item in the swatch panel.
        /// </summary>
        private void EditSingleTarget () {

            //Get the selection
            GameObject target = Selection.activeGameObject;

            if (!target.GetComponent<ChunkObjectData>()) {

                Debug.LogWarning("[Edit Mode]: Target selected has no ChunkObjectData component. Thereby it isn't viewed as one.");

            }
            else {

                //Get selected item in Swatch.
                GameObject selectedSwatchItem = voxelSwatchReference.GetSelectedTile();

                //Replace the target with the selectedSwatch item.
                VMEMainWindow.Instance.tileAddControls.EditTile(selectedSwatchItem, target);

            }

        }

        /// <summary>
        /// Change all the selected chunkObjects to selected item in the swatch panel.
        /// </summary>
        private void EditMultipleTargets () {

            //Get the selection
            GameObject[] targets = Selection.gameObjects;

            foreach (GameObject target in targets) {

                if (!target.GetComponent<ChunkObjectData>()) {

                    Debug.LogWarning("[Edit Mode]: Target selected has no ChunkObjectData component. Thereby it isn't viewed as one.");

                }
                else {

                    //Get selected item in Swatch.
                    GameObject selectedSwatchItem = voxelSwatchReference.GetSelectedTile();

                    //Replace the target with the selectedSwatch item.
                    VMEMainWindow.Instance.tileAddControls.EditTile(selectedSwatchItem, target);

                }

            }

        }

        /// <summary>
        /// Tries to switch the tile to another style of that tile.
        /// </summary>
        private void TryToSwitchTileStyle () {
            //Get the tile i'm pointing at.
            GameObject target = VMEGlobal.GetTileAtMousePosition();

            if (target == null) {

                Debug.LogWarning("[Edit Mode - SwapStyle]: No target selected.");

            }
            else {

                VSTileGroup group = voxelSwatchReference.GetSwatch().GetGroupByName(target.name);

                if (group == null) {

                    Debug.LogWarning("[Edit Mode - SwapStyle]: This tile is not a item from the VoxelSwatch. Style swap not possible.");

                }
                else {

                    GameObject[] itemsInGroup = group.styles.ToArray();

                    if (itemsInGroup.Length == 1) {

                        Debug.LogWarning("[Edit Mode - SwapStyle]: The Tile of type: " + target.name + " Only has one variation.");

                    }
                    else {

                        int currentIndexInGroup = -1;
                        Debug.Log("Items in the group: " + itemsInGroup.Length);
                        for (int i = 0; i < itemsInGroup.Length; i++) {

                            if (target.name == itemsInGroup[i].name) {

                                currentIndexInGroup = i;
                                Debug.Log(currentIndexInGroup);
                            }

                        }


                        int newIndex = -1;

                        if (currentIndexInGroup != -1) {
                            Debug.Log(currentIndexInGroup + " : " + itemsInGroup.Length);
                            if (currentIndexInGroup + 1 == itemsInGroup.Length) {

                                newIndex = 0;

                            }
                            else {

                                newIndex = currentIndexInGroup + 1;

                            }

                            if (currentIndexInGroup != newIndex) {

                                VMEMainWindow.Instance.tileAddControls.EditTile(itemsInGroup[newIndex].gameObject, target);

                            }

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Tries to rotate the tile that is behind mouse position.
        /// </summary>
        /// <param name="degrees">the degree value the tile will be rotated by.</param>
        private void TryToRotateTile (int degrees) {

            GameObject hitObject = VMEGlobal.GetTileAtMousePosition();

            if (hitObject != null) {

                hitObject.transform.Rotate(new Vector3(0, degrees, 0));

            }

        }

    }

}
