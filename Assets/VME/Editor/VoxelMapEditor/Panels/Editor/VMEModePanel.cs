using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;
using System;
using System.Reflection;

namespace VME {

    public class VMEModePanel : BaseEditorPanel {

        public VMEModePanel (EditorWindow _window, KeyCode _toggleKey, VMEVoxelSwatchPanel _swatchPanel) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;
            swatchPanelReference = _swatchPanel;
            editControls = new VMEEditControls(_swatchPanel);
            paintControls = new VMEPaintControls(_swatchPanel);

        }

        //---Textures----------------------------------->

        private Texture moveIcon;
        private Texture editIcon;
        private Texture paintIcon;
        private Texture removeIcon;
        private Texture selectionIcon;

        //---------------------------------------------->

        /// <summary>
        /// Reference to the Object that holds the editor settings.
        /// </summary>
        private VMEVoxelSwatchPanel swatchPanelReference;

        /// <summary>
        /// The controls for making a selection.
        /// </summary>
        private VMESelectionControls selectionControls = new VMESelectionControls();

        /// <summary>
        /// The controls for painting tiles.
        /// </summary>
        private VMEPaintControls paintControls;

        /// <summary>
        /// The controls for editing a tile.
        /// </summary>
        private VMEEditControls editControls;

        /// <summary>
        /// Names of each mode.
        /// </summary>
        private string[] modes = new string[]  {"Move" , "Select" , "Edit" , "Paint" , "Remove" };

        /// <summary>
        /// Current index of mode being used.
        /// </summary>
        private int currentModeIndex = 0;

        #region BaseEditorWindow Functions

        public override void DrawContent () {

            base.DrawContent();

            EditorGUILayout.BeginHorizontal();

            DrawModeSelectPanel();

            EditorGUILayout.BeginVertical();

            DrawInfoPanel();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

        }

        public override void Input (SceneView sceneView) {

            base.Input(sceneView);

            Event e = Event.current;

            if (e.isKey) {

                if (e.type == EventType.KeyDown) {

                    if (e.keyCode == settingsObject.SET_MODE_TO_SELECT) { currentModeIndex = 0; VMEGlobal.Hidden = false; }
                    if (e.keyCode == settingsObject.SET_MODE_TO_MOVE) { currentModeIndex = 1; VMEGlobal.Hidden = true; }
                    if (e.keyCode == settingsObject.SET_MODE_TO_EDIT) { currentModeIndex = 2; VMEGlobal.Hidden = true; }
                    if (e.keyCode == settingsObject.SET_MODE_TO_PAINT) { currentModeIndex = 3; VMEGlobal.Hidden = true; }
                    if (e.keyCode == settingsObject.SET_MODE_TO_REMOVE) { currentModeIndex = 4; VMEGlobal.Hidden = true; }

                }

            }

            switch (currentModeIndex) {

                case 1: selectionControls.Input(sceneView); break;
                case 0: break;
                case 2: editControls.Input(sceneView); break;
                case 3: paintControls.Input(sceneView); break;
                case 4: break;

            }

        }

        #endregion

        #region UI

        public override void OnSceneUI (SceneView _sceneView) {

            base.OnSceneUI(_sceneView);

            Handles.BeginGUI();

            GUI.DrawTexture(new Rect(0, 0, 50, 50), getTexture());
            GUI.Label(new Rect(0,50,50,100),modes[currentModeIndex]);

            Handles.EndGUI();

        }

        /// <summary>
        /// Gets the textures from the VME folder to draw.
        /// </summary>
        /// <returns>The icon of the current mode being used.</returns>
        private Texture getTexture () {
            if(removeIcon == null) { removeIcon = AssetDatabase.LoadAssetAtPath("Assets/VME/Textures/Remove.png", typeof(Texture)) as Texture; }
            if (editIcon == null) { editIcon = AssetDatabase.LoadAssetAtPath("Assets/VME/Textures/Edit.png", typeof(Texture)) as Texture; }
            if (paintIcon == null) { paintIcon = AssetDatabase.LoadAssetAtPath("Assets/VME/Textures/Paint.png", typeof(Texture)) as Texture; }
            if (moveIcon == null) { moveIcon = AssetDatabase.LoadAssetAtPath("Assets/VME/Textures/Move.png", typeof(Texture)) as Texture; }
            if (selectionIcon == null) { selectionIcon = AssetDatabase.LoadAssetAtPath("Assets/VME/Textures/Selection.png", typeof(Texture)) as Texture; }

            switch (currentModeIndex) {

                case 0: return selectionIcon;
                case 1: return moveIcon;
                case 2: return editIcon;
                case 3: return paintIcon;
                case 4: return removeIcon;

            }

            return removeIcon;
        }

        /// <summary>
        /// Draws mode selection panel.
        /// </summary>
        private void DrawModeSelectPanel () {
            
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < 4; i++) {

                if (currentModeIndex == i) {

                   // GUILayout.Label(modes[i]);

                } else {

                    if (GUILayout.Button(modes[i], GUILayout.Width(70))) { currentModeIndex = i; }

                }

            }

            EditorGUILayout.EndVertical();

        }

        /// <summary>
        /// Draws information of each mode.
        /// </summary>
        private void DrawInfoPanel () {

            switch (currentModeIndex) {

                case 0:
                EditorGUILayout.HelpBox("Move mode, use normal editor controls for moving objects", MessageType.Info);
                break;

                case 1:
                EditorGUILayout.HelpBox("Selection mode, use normal editor controls for moving objects", MessageType.Info);
                break;

                case 2:
                EditorGUILayout.HelpBox("Edit mode, Select objects to edit and press the apply button or key.", MessageType.Info);
                break;

                case 3:
                EditorGUILayout.HelpBox("Paint mode, Hover over location to paint and press the paint key.", MessageType.Info);
                break;

                case 4:
                EditorGUILayout.HelpBox("Remove mode, Select objects to remove and press the apply button or key.", MessageType.Info);
                break;

            }
          
        }

        #endregion

    }

}

