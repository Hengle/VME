using UnityEngine;
using System.Collections;
using UnityEditor;
//Used for Calling the Init function after reload.
using UnityEditor.Callbacks;

namespace VSE {

    public class VSEWindow : EditorWindow {

        #region Variables

        /// <summary>
        /// Static reference of the Voxel Swatch Editor.
        /// </summary>
        public static VSEWindow Instance { get; private set; }

        //---Panels----------------------------------->

        public static VSEHeader header;
        public static VSESwatchCategoryPanel swatchCategory;
        public static VSEGroupInspector groupInspector;

        //-------------------------------------------->

        /// <summary>
        /// The backgroundTexture of the Voxel Swatch Editor.
        /// </summary>
        private static Texture2D backgroundTexture = null;

        #endregion


        #region Unity Functions

        [MenuItem("VME/Open/Swatch Editor")]
        [DidReloadScripts]
        static void Init () {

            VSEWindow window = (VSEWindow)EditorWindow.GetWindow(typeof(VSEWindow));
            Instance = window;

            //create background texture.
            backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            backgroundTexture.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f, 1));
            backgroundTexture.Apply();

            header = new VSEHeader(window);
            swatchCategory = new VSESwatchCategoryPanel(window);
            groupInspector = new VSEGroupInspector(window);

            window.Show();
            window.titleContent.text = "Voxel Swatch Editor";

        }

        /// <summary>
        /// For repainting the Window.
        /// </summary>
        public void Update () {

            Repaint();

        }

        /// <summary>
        /// For drawing the editor.
        /// </summary>
        void OnGUI () {

            //Draw background
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundTexture, ScaleMode.StretchToFill);

            header.Draw();

            //Draw the category that currently is selected.
            switch (header.CurrentState) {

                case VSEHeader.SelectedCategoryState.General:

                break;


                case VSEHeader.SelectedCategoryState.Swatch:

                EditorGUILayout.BeginHorizontal();
                swatchCategory.Draw();
                groupInspector.Draw();
                EditorGUILayout.EndHorizontal();

                break;


                case VSEHeader.SelectedCategoryState.Etc:

                break;

            }

        }

        /// <summary>
        /// Used for registering Events
        /// </summary>
        void OnFocus () {

            if (header != null) {

                ReConnectEvents();

            }

        }

        #endregion

        #region Functions

        /// <summary>
        /// Removes and adds all listeners.
        /// </summary>
        private void ReConnectEvents () {

            //header.OnVoxelSwatchChangedEvent -= OnVoxelSwatchChangedEvent;
            header.OnVoxelSwatchChangedEvent -= OnVoxelSwatchChangedEvent;
            header.OnVoxelSwatchChangedEvent += OnVoxelSwatchChangedEvent;
            header.OnVoxelSwatchChangedEvent -= swatchCategory.OnVoxelSwatchChanged;
            header.OnVoxelSwatchChangedEvent += swatchCategory.OnVoxelSwatchChanged;
            swatchCategory.OnGroupChanged -= groupInspector.OnTileGroupChanged;
            swatchCategory.OnGroupChanged += groupInspector.OnTileGroupChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// When the group is changed.
        /// </summary>
        /// <param name="_itemGroup">the new group.</param>
        private void OnGroupChanged (VSTileGroup _itemGroup) {



        }

        /// <summary>
        /// When the voxel swatch is changed.
        /// </summary>
        /// <param name="_swatch">the new swatch.</param>
        private void OnVoxelSwatchChangedEvent (VoxelSwatch _swatch) {



        }

        #endregion

    }

}
