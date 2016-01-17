using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

namespace VME {

    public class VMEFileManagerPanel : BaseEditorPanel {

        public delegate void VoxelMapAction (VoxelMap _map);
        public event VoxelMapAction OnVoxelMapChangedEvent;

        public delegate void VoxelSwatchAction (VoxelSwatch _swatch);
        public event VoxelSwatchAction OnVoxelSwatchChangedEvent;

        public VoxelMap voxelMap;
        public VoxelSwatch voxelSwatch;

        public VMEFileManagerPanel (EditorWindow _window, KeyCode _toggleKey) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;

        }

        public override void DrawContent () {
            base.DrawContent();

            VoxelMap map = voxelMap;
            voxelMap = (VoxelMap)EditorGUILayout.ObjectField(voxelMap, typeof(VoxelMap), true);
            if (voxelMap != map) {

                OnVoxelMapChanged();

            }

            if (voxelMap != null) {

                VoxelSwatch swatch = voxelSwatch;
                voxelSwatch = (VoxelSwatch)EditorGUILayout.ObjectField(voxelSwatch, typeof(VoxelSwatch), true);

                if (voxelSwatch != swatch) {

                    OnVoxelSwatchChanged();

                }

                if (voxelSwatch != null) {

                    if (GUILayout.Button("Reload Swatch.")) {

                        voxelSwatch.LoadSwatches();

                    }

                }

            }

        }

        public override void Input (SceneView sceneView) {

            base.Input(sceneView);

        }


        #region Functions

        private void OnVoxelMapChanged () {
            
            if (voxelMap != null) {

                voxelSwatch = voxelMap.voxelSwatch;
               
            }
            else {
              
                
            }

            if (OnVoxelMapChangedEvent != null) {

                OnVoxelMapChangedEvent(voxelMap);

            }

        }

        private void OnVoxelSwatchChanged () {

            voxelMap.voxelSwatch = voxelSwatch;

            if (OnVoxelSwatchChangedEvent != null) {

                OnVoxelSwatchChangedEvent(voxelMap.voxelSwatch);

            }

        }

        #endregion

    }

}

