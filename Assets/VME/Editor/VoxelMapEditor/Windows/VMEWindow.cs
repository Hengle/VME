using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;

namespace VME {

    /// <summary>
    /// Voxel Map Main Editor window.
    /// 
    /// Holds all the panels and other classes for the editor,
    /// Connects the functions used in panels ( ex. Editor Input / Editor Draw )
    /// </summary>
    public class VMEMainWindow : EditorWindow {

        public static VMEMainWindow Instance { get; private set; }

        //---Panels----------------------------------->

        public static VMEMainPanel mainPanel;
        public static VMESelectionPanel selectionPanel;
        public static VMEVoxelSwatchPanel swatchPanel;
        public static VMEFileManagerPanel fileManager;
        public static VMEModePanel modePanel;
        public static VMEChunkPanel chunkPanel;

        //-------------------------------------------->

        public VMETileAddControls tileAddControls;

        private static Texture2D tex = null;

        /// <summary>
        /// Reference to the Object that holds the editor settings.
        /// </summary>
        private static VMESettingsObject settingsObject;

        /// <summary>
        /// Current scroll position of the editor window.
        /// </summary>
        private Vector2 scrollPosition;

        #region Unity Functions

        [MenuItem("VME/Open/Editor")]
        [DidReloadScripts]
        static void Init () {

            VMEMainWindow window = (VMEMainWindow)EditorWindow.GetWindow(typeof(VMEMainWindow));
            Instance = window;

            //create background color.
            tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f, 1));
            tex.Apply();

            window.Show();
            window.titleContent.text = "VME";

            settingsObject = VMESettingsObject.LoadScriptableObject();
            mainPanel = new VMEMainPanel(window, settingsObject.TOGGLE_EDITOR);
            swatchPanel = new VMEVoxelSwatchPanel(window, settingsObject.ENABLE_SWATCH);
            fileManager = new VMEFileManagerPanel(window, settingsObject.ENABLE_FILEMANAGER);
            modePanel = new VMEModePanel(window, settingsObject.ENABLE_MODE, swatchPanel);
            chunkPanel = new VMEChunkPanel(window, settingsObject.ENABLE_CHUNK);
            selectionPanel = new VMESelectionPanel();

            

        }

        /// <summary>
        /// For repainting the Window.
        /// </summary>
        public void Update () {
            Repaint();
        }

        /// <summary>
        /// Used for registering Events
        /// </summary>
        void OnFocus () {

            if (tileAddControls == null) {

                tileAddControls = new VMETileAddControls();

            }

            if (mainPanel != null) {

                ReConnectEvents();

            }

        }

       
        /// <summary>
        /// Used for drawing the editor.
        /// </summary>
        void OnGUI () {

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            
            selectionPanel.Draw();
            mainPanel.Draw("Main");
            chunkPanel.Draw("Chunk Selection");
            modePanel.Draw("Mode Selection");
            swatchPanel.Draw("Swatch");
            fileManager.Draw("FileManager");
            
            EditorGUILayout.EndScrollView();

        }

        #endregion

        #region Functions

        /// <summary>
        /// Removes and adds all listeners.
        /// </summary>
        private void ReConnectEvents () {

            mainPanel.RemoveEventlisteners(SceneView.currentDrawingSceneView);
            mainPanel.AddEventListeners(SceneView.currentDrawingSceneView);

            fileManager.RemoveEventlisteners(SceneView.currentDrawingSceneView);
            fileManager.AddEventListeners(SceneView.currentDrawingSceneView);
            fileManager.OnVoxelMapChangedEvent -= FileManager_OnVoxelMapChangedEvent;
            fileManager.OnVoxelMapChangedEvent += FileManager_OnVoxelMapChangedEvent;
            fileManager.OnVoxelSwatchChangedEvent -= FileManager_OnVoxelSwatchChangedEvent;
            fileManager.OnVoxelSwatchChangedEvent += FileManager_OnVoxelSwatchChangedEvent;

            swatchPanel.RemoveEventlisteners(SceneView.currentDrawingSceneView);
            swatchPanel.AddEventListeners(SceneView.currentDrawingSceneView);

            modePanel.RemoveEventlisteners(SceneView.currentDrawingSceneView);
            modePanel.AddEventListeners(SceneView.currentDrawingSceneView);

            chunkPanel.RemoveEventlisteners(SceneView.currentDrawingSceneView);
            chunkPanel.AddEventListeners(SceneView.currentDrawingSceneView);

        }

        #endregion

        #region Events

        /// <summary>
        /// Called when the swatch is changed.
        /// </summary>
        /// <param name="_swatch">The swatch that it is changed into.</param>
        private void FileManager_OnVoxelSwatchChangedEvent (VoxelSwatch _swatch) {

            if (_swatch != null) {//If theres no swatch selected.

                chunkPanel.SetState(true);
                swatchPanel.SetState(true);
                swatchPanel.SetVoxelSwatch(_swatch);

            } else {//If there is a swatch selected.

                chunkPanel.SetState(false);
                swatchPanel.SetState(false);
                swatchPanel.SetVoxelSwatch(null);

            }

        }

        /// <summary>
        /// Called when the VoxelMap is changed.
        /// </summary>
        /// <param name="_map">The map that it is changed into.</param>
        private void FileManager_OnVoxelMapChangedEvent (VoxelMap _map) {

            if (_map != null) {//If theres no map selected.

                chunkPanel.SetState(true);
                swatchPanel.SetState(true);
                swatchPanel.SetVoxelSwatch(_map.voxelSwatch);
                chunkPanel.SetVoxelMap(_map);

            } else {//If there is a map selected.

                chunkPanel.SetState(false);
                swatchPanel.SetState(false);
                swatchPanel.SetVoxelSwatch(null);
                chunkPanel.SetVoxelMap(null);

            }

        }

        #endregion

    }

}
