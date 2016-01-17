using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VME {

    /// <summary>
    /// Controls for painting a tile.
    /// </summary>
    public class VMEPaintControls {

        public VMEPaintControls (VMEVoxelSwatchPanel _voxelswatchpanel) {

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

                    if (e.keyCode == settingsObject.APPLY_SINGLE) {

                        PaintAtHoverPosition();

                    }

                    if (e.keyCode == settingsObject.APPLY_ALL) {

                        //paint all of selection

                    }

                }

            }

        }

        public void PaintAtHoverPosition () {

            //Get GameObject that we hovered over to check if we can paint,
            //also use its rotation for the new tile.
            GameObject hoveredTile = VMEGlobal.GetTileAtMousePosition();

            //if theres none.
            if(hoveredTile == null) {

                Debug.LogWarning("[Paint Mode]: Not hovered over a tile, thereby can't paint a tile.");

            } else {

                //Get selected item in Swatch.
                GameObject selectedSwatchItem = voxelSwatchReference.GetSelectedTile();

                //Get position next to hovered tile.
                Vector3 newPosition = VMEGlobal.GetPositionNextToHoveredTile();

                //Paint the tile.
                VMEMainWindow.Instance.tileAddControls.PaintTIle(selectedSwatchItem, hoveredTile, newPosition);

            }

        }

        public void PaintAllInsideSelection () {

            Debug.LogError("[Paint Mode] : Selection Paint not yet implemented.");

        }

    }

}