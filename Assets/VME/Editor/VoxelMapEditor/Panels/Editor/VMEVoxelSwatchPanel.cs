using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace VME {

    public class VMEVoxelSwatchPanel : BaseEditorPanel {

        private EditorSearchWindow searchWindow;
        private VoxelSwatch voxelSwatch;
        private GameObject selectedItem;

        //Category Selection
        private Editor preview;
        private Vector2 scrollPosition;
        private int categoryCount = 0;
        private int currentCategory = 0;
        //////////////////////////////

        public VMEVoxelSwatchPanel (EditorWindow _window,KeyCode _toggleKey) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;
            settingsObject = VMESettingsObject.LoadScriptableObject();
            searchWindow = new EditorSearchWindow(settingsObject.SWATCH_ENABLE_SEARCH, settingsObject.SWATCH_ITEM_INCREASE, settingsObject.SWATCH_ITEM_INCREASE, base.parentWindow);
            searchWindow.OnItemSelected += SearchWindow_OnItemSelected;

        }

        public override void OnOpened () {

            base.OnOpened();

            if(voxelSwatch != null) {

                categoryCount = voxelSwatch.categories.Count;

            }

        }

        private void SearchWindow_OnItemSelected (int _index) {
        
            selectedItem = voxelSwatch.categories[currentCategory].tilegroups[_index].styles[0];
            preview = Editor.CreateEditor(selectedItem);

        }

        public override void DrawContent () {

            base.DrawContent();

            if (voxelSwatch != null) {

                EditorInput();

                if (preview != null) {

                    preview.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.helpBox);
                    
                }

                DrawModeSelectionPanel();

                if (voxelSwatch.categories[currentCategory].tilegroups.Count != 0) {

                    EditorUI.Draw.PressedButton("Currently Selected : " + voxelSwatch.categories[currentCategory].tilegroups[searchWindow.GetSelectedIndex()].styles[0].name);
              
                    
                } else {

                    EditorUI.Draw.PressedButton("Currently Selected : None");

                }

                searchWindow.Draw("Search");

            } else {

                EditorGUILayout.HelpBox("No VoxelSwatch Loaded. Import a VoxelMap in the FileManager Panel", MessageType.Error);

            }

        }

        private void EditorInput () {

            Event e = Event.current;

            if (e.isKey) {

                if (e.type == EventType.KeyDown) {

                    if (e.keyCode == settingsObject.SWATCH_SELECT_CATEGORY_DECREASE) {

                        if(currentCategory != 0) {

                            currentCategory--;

                        }

                    }

                    if (e.keyCode == settingsObject.SWATCH_SELECT_CATEGORY_INCREASE) {

                        if (currentCategory != voxelSwatch.categories.Count) {

                            currentCategory++;

                        }

                    }

                }

            }

        }

        public override void Input (SceneView sceneView) {

            base.Input(sceneView);
            searchWindow.Input(sceneView);

            Event e = Event.current;

            if (e.isKey) {

                if (e.keyCode == KeyCode.I) {
                    
                    GameObject target = VMEGlobal.GetTileAtMousePosition();

                    if (target != null) {

                        SetSwatchSelection(target);

                    }

                }

            }

        }

        #region UI

        private void DrawModeSelectionPanel () {

            if (categoryCount != voxelSwatch.categories.Count) {

                categoryCount = voxelSwatch.categories.Count;

            }

            if (categoryCount > 3) {

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(40));
                
            } else {

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(28));

            }

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < voxelSwatch.categories.Count; i++) {

                if (currentCategory == i) {

                    EditorUI.Draw.PressedButton(voxelSwatch.categories[i].categoryName);
                    
                } else {

                    if (GUILayout.Button(voxelSwatch.categories[i].categoryName, GUILayout.Width(113))) {

                        currentCategory = i;
                        SetCategory(i);

                    }

               }

            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();

        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets a new VoxelSwatch.
        /// </summary>
        /// <param name="_swatch">The Swatch</param>
        public void SetVoxelSwatch (VoxelSwatch _swatch) {

            voxelSwatch = _swatch;

            if (_swatch != null) {

                categoryCount = voxelSwatch.categories.Count;
               

            } else {

                categoryCount = 0;

            }

            currentCategory = 0;
            SetCategory(currentCategory);

        }

        public void SetCategory(int categoryIndex) {

            List<string> names = new List<string>();
            if (voxelSwatch != null) {

                if (voxelSwatch.categories == null) {

                    Debug.LogWarning("The voxelSwatch has no categories");

                } else {

                    for (int i = 0; i < voxelSwatch.categories[categoryIndex].tilegroups.Count; i++) {

                        names.Add(voxelSwatch.categories[categoryIndex].tilegroups[i].styles[0].name);

                    }

                }

            searchWindow.SetContent(names);

            }

        }

        public string GetCategoryName () {

            return voxelSwatch.categories[currentCategory].categoryName;

        }

        public void SetSwatchSelection (GameObject obj) {
            string[] splitName = obj.name.Split(' ');

            if(splitName.Length == 2) {

                obj.name = splitName[0];

            }
           
            string categoryNameOfSelection = voxelSwatch.GetGroupByName(obj.name).categoryReference.categoryName;

            switch (categoryNameOfSelection) {
                case "Tiles":
                    SetCategory(0);
                break;
                case "Props":
                    SetCategory(1);
                break;
                case "Effects":
                    SetCategory(2);
                break;
            }

            //Go through item to set the selection
            List<string> items = searchWindow.GetCurrentListOfItems();

            for(int i = 0; i < items.Count; i++) {

                if(items[i] == obj.name) {

                    searchWindow.ChangeSelectionByID(i);

                }

            }

        }

        /// <summary>
        /// Returns the item that is selected in this SwatchPanel.
        /// </summary>
        /// <returns>Item selected</returns>
        public GameObject GetSelectedTile () {

            return selectedItem;

        }

        public VoxelSwatch GetSwatch () {

            return voxelSwatch;

        }

        #endregion

    }

}
