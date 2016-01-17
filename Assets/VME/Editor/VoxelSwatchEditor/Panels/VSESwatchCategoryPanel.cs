using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace VSE {

    /// <summary>
    /// Panel that displays the VoxelSwatch categories and the TileGroups inside the selected category.
    /// </summary>
    public class VSESwatchCategoryPanel : BaseEditorGroup {

        public VSESwatchCategoryPanel (EditorWindow _window) {

            base.parentWindow = _window;
            groupSearchWindow = new EditorSearchGroup(175);
            groupSearchWindow.OnItemSelected += OnTileGroupSelected;

        }

        #region Variables

        /// <summary>
        /// Reference to the VoxelSwatch chosen in the VSEHeader.
        /// </summary>
        public VoxelSwatch swatchReference;

        /// <summary>
        /// Delegate for the OnGroupChanged event.
        /// </summary>
        /// <param name="_tileGroup"></param>
        public delegate void TileGroupAction (VSTileGroup _tileGroup);

        /// <summary>
        /// Called when the TileGroup is changed in this Panel.
        /// </summary>
        public event TileGroupAction OnGroupChanged;

        /// <summary>
        /// The search window that displays the Styles inside the TileGroup.
        /// </summary>
        private EditorSearchGroup groupSearchWindow;

        /// <summary>
        /// The name used for when a new TileGroup is created.
        /// </summary>
        private string newGroupName = "";

        /// <summary>
        /// The TileGroup that is selected in this Panel.
        /// </summary>
        private VSTileGroup selectedTileGroup = null;

        /// <summary>
        /// Current index of the selectedTileGroup in the TileGroup List.
        /// </summary>
        private int categoryIndex;
        
        #endregion


        #region BaseEditorGroup Functions

        public override void DrawContent () {

            base.DrawContent();

            if (swatchReference != null) {

                DrawSidePanel();

            }

        }

        #endregion

        #region UI

        /// <summary>
        /// Draws the SidePanel, containing the Search Panel and the buttons to modify the TileGroup List.
        /// </summary>
        private void DrawSidePanel () {

            EditorGUILayout.BeginVertical("box", GUILayout.Width(150));

            DrawCategoryButtons();
            groupSearchWindow.Draw();
            DrawListModifyPanel();

            EditorGUILayout.EndVertical();

        }

        /// <summary>
        /// Draws the Add and Remove TileGroup buttons.
        /// </summary>
        private void DrawListModifyPanel () {

            newGroupName = EditorGUILayout.TextField(newGroupName);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove")) {

                VSTileGroup group = swatchReference.GetGroupByShortenedName(newGroupName);

                if (group == null) {

                    Debug.LogError("[VoxelSwatch Editor - Removing TileGroup]: not found TileGroup by name: " + newGroupName);

                } else {

                    swatchReference.categories[categoryIndex].RemoveByName(newGroupName);
                    UpdateGroupList();

                }

            }

            if (GUILayout.Button("Add")) {

                if (swatchReference.categories[categoryIndex].CheckGroup(newGroupName) == null) {

                    swatchReference.categories[categoryIndex].CreateNewTileGroup(newGroupName);

                    UpdateGroupList();

                }

            }

            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// Draws each Category Button in the VoxelSwatch.
        /// </summary>
        private void DrawCategoryButtons () {

            EditorGUILayout.BeginHorizontal();

            Color normalColor = GUI.color;

            for (int i = 0; i < swatchReference.categories.Count; i++) {

                if (i == categoryIndex) {

                    GUI.color = Color.yellow;
                    if (GUILayout.Button(swatchReference.categories[i].categoryName)) { }
                    GUI.color = normalColor;

                } else {

                    if (GUILayout.Button(swatchReference.categories[i].categoryName)) {

                        SetCategory(i);

                    }

                }

            }

            EditorGUILayout.EndHorizontal();

        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets the Tile Category.
        /// </summary>
        /// <param name="_categoryIndex">Index of the Category.</param>
        private void SetCategory (int _categoryIndex) {

            categoryIndex = _categoryIndex;

            UpdateGroupList();

        }

        /// <summary>
        /// Updates the TileGroup List and sends it to the Search Panel.
        /// </summary>
        private void UpdateGroupList () {

            List<string> TileNames = new List<string>();

            for (int i = 0; i < swatchReference.categories[categoryIndex].tilegroups.Count; i++) {

                string newstring = swatchReference.categories[categoryIndex].tilegroups[i].groupName;

                TileNames.Add(newstring);

            }

            groupSearchWindow.SetContent(TileNames);

        }

        #endregion

        #region Events

        /// <summary>
        /// When the voxel swatch is changed
        /// </summary>
        /// <param name="_swatch">the new swatch</param>
        public void OnVoxelSwatchChanged (VoxelSwatch _swatch) {

            swatchReference = _swatch;

            if (swatchReference != null) {

                SetCategory(0);
                swatchReference.LoadSwatches();

            }

        }

        /// <summary>
        /// Called when a tile group is selected in the search panel.
        /// </summary>
        /// <param name="_index">The index of the item in its original list.</param>
        private void OnTileGroupSelected (int _index) {

            selectedTileGroup = swatchReference.categories[categoryIndex].tilegroups[_index];
            OnGroupChanged(selectedTileGroup);

        }

        #endregion

    }

}
