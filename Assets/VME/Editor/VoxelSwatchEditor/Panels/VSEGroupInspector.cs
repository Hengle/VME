using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace VSE {

    /// <summary>
    /// Inspector displaying the content of given TileGroup.
    /// </summary>
    public class VSEGroupInspector : BaseEditorGroup {

        public VSEGroupInspector (EditorWindow _window) {

            base.parentWindow = _window;
            styleSearchWindow = new EditorSearchGroup(150);
            styleSearchWindow.OnItemSelected += OnStyleSelected;

        }

        #region Variables

        /// <summary>
        /// The search window that displays the Styles inside the TileGroup.
        /// </summary>
        private EditorSearchGroup styleSearchWindow;

        /// <summary>
        /// The TileGroup currently selected.
        /// </summary>
        private VSTileGroup currentTileGroup = null;

        /// <summary>
        /// The Style selected in this window.
        /// </summary>
        private GameObject selectedTileStyle = null;

        /// <summary>
        /// Current index of the selectedTileStyle in the StyleGroup List.
        /// </summary>
        private int styleIndex;

        #endregion


        #region BaseEditorGroup Functions

        public override void DrawContent () {

            base.DrawContent();

            if (currentTileGroup != null) {

                EditorGUILayout.BeginVertical();
                DrawHeader();
                DrawSidePanel();
                EditorGUILayout.EndVertical();

            }

        }

        #endregion

        #region UI

        /// <summary>
        /// Draws the header, containing the name of the currently selected TileGroup.
        /// </summary>
        private void DrawHeader () {

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorUI.Draw.TitleField(currentTileGroup.groupName);
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// Draws the SidePanel, containing the Search Panel and the buttons to modify the Style List.
        /// </summary>
        private void DrawSidePanel () {

            EditorGUILayout.BeginVertical("box", GUILayout.Width(150));

            styleSearchWindow.Draw();
            DrawListModifyPanel();

            EditorGUILayout.EndVertical();

        }

        /// <summary>
        /// Draws the Add and Remove Style buttons.
        /// </summary>
        private void DrawListModifyPanel () {

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Remove Selected")) {

                if (currentTileGroup.styles.Count > 1) {

                    currentTileGroup.RemoveStyle(selectedTileStyle);
                    UpdateStyleList();

                }

            }

            if (GUILayout.Button("Add")) {
                
                currentTileGroup.AddNewStyle(currentTileGroup.groupName, currentTileGroup.styles.Count + 1);
                UpdateStyleList();

            }

            EditorGUILayout.EndVertical();

        }

        #endregion

        #region Functions

        /// <summary>
        /// Updates the Style List and sends it to the Search Panel.
        /// </summary>
        private void UpdateStyleList () {

            List<string> TileNames = new List<string>();

            for (int i = 0; i < currentTileGroup.styles.Count; i++) {

                string newstring = currentTileGroup.styles[i].name;

                TileNames.Add(newstring);

            }

            styleSearchWindow.SetContent(TileNames);

        }

        #endregion

        #region Events

        /// <summary>
        /// Called when a style is selected in the Search Window.
        /// </summary>
        /// <param name="_index">The index of the selected item in the full Style List.</param>
        private void OnStyleSelected (int _index) {

            styleIndex = _index;
            selectedTileStyle = currentTileGroup.styles[_index];

        }

        /// <summary>
        /// Called when the TileGroup is changed in the VSESwatchCategoryPanel.
        /// </summary>
        /// <param name="_group">The new TileGroup.</param>
        public void OnTileGroupChanged (VSTileGroup _group) {

            currentTileGroup = _group;
            UpdateStyleList();

        }

        #endregion

    }

}
