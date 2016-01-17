using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VSE {

    /// <summary>
    /// Contains the Swatch input field and category selection buttons
    /// </summary>
    public class VSEHeader : BaseEditorGroup {

        public VSEHeader (EditorWindow _window) {

            base.parentWindow = _window;

        }

        #region Variables

        public enum SelectedCategoryState {
            NoSwatchSelected,
            None,
            General,
            Swatch,
            Etc
        }

        private SelectedCategoryState _currentState = SelectedCategoryState.NoSwatchSelected;
        public SelectedCategoryState CurrentState {

            get {

                return _currentState;

            }

            set {

                _currentState = value;

                if (OnHeaderCategorySwitched != null)
                    OnHeaderCategorySwitched(_currentState);

            }

        }

        private VoxelSwatch _voxelSwatch = null;
        public VoxelSwatch VoxelSwatch {

            get {

                return _voxelSwatch;

            }

            set {

                if (_voxelSwatch != value) {

                    if (OnVoxelSwatchChangedEvent != null) {

                        OnVoxelSwatchChangedEvent(value);

                    }

                    CurrentState = SelectedCategoryState.None;

                }

                _voxelSwatch = value;

            }

        }

        public delegate void VoxelSwatchAction (VoxelSwatch _swatch);
        public event VoxelSwatchAction OnVoxelSwatchChangedEvent;

        public delegate void CategorySwitchAction (SelectedCategoryState _state);
        public event CategorySwitchAction OnHeaderCategorySwitched;

        #endregion        

        #region BaseEditorGroup Functions

        public override void DrawContent () {

            base.DrawContent();

            EditorGUILayout.BeginHorizontal("box");

            VoxelSwatch = (VoxelSwatch)EditorGUILayout.ObjectField(VoxelSwatch, typeof(VoxelSwatch), false, GUILayout.Width(150));

            DrawHeaderButtons();

            EditorGUILayout.EndHorizontal();

        }

        #endregion

        #region UI

        /// <summary>
        /// Draws the row of buttons in this header.
        /// </summary>
        private void DrawHeaderButtons () {

            Color normalCol = GUI.color;

            switch (CurrentState) {

                case SelectedCategoryState.NoSwatchSelected:

                GUI.color = new Color(0.2f, 0.2f, 0.2f);
                if (GUILayout.Button("General")) { }
                if (GUILayout.Button("Swatch")) { }
                if (GUILayout.Button("Etc")) { }
                GUI.color = normalCol;

                break;

                case SelectedCategoryState.None:


                if (GUILayout.Button("General")) {

                    CurrentState = SelectedCategoryState.General;

                }

                if (GUILayout.Button("Swatch")) {

                    CurrentState = SelectedCategoryState.Swatch;

                }

                if (GUILayout.Button("Etc")) {

                    CurrentState = SelectedCategoryState.Etc;

                }

                break;

                case SelectedCategoryState.General:

                GUI.color = Color.yellow;
                if (GUILayout.Button("General")) { }
                GUI.color = normalCol;

                if (GUILayout.Button("Swatch")) {

                    CurrentState = SelectedCategoryState.Swatch;

                }

                if (GUILayout.Button("Etc")) {

                    CurrentState = SelectedCategoryState.Etc;

                }

                break;

                case SelectedCategoryState.Swatch:


                if (GUILayout.Button("General")) {

                    CurrentState = SelectedCategoryState.General;

                }

                GUI.color = Color.yellow;
                if (GUILayout.Button("Swatch")) { }
                GUI.color = normalCol;

                if (GUILayout.Button("Etc")) {

                    CurrentState = SelectedCategoryState.Etc;

                }

                break;

                case SelectedCategoryState.Etc:


                if (GUILayout.Button("General")) {

                    CurrentState = SelectedCategoryState.General;

                }

                if (GUILayout.Button("Swatch")) {

                    CurrentState = SelectedCategoryState.Swatch;

                }

                GUI.color = Color.yellow;
                if (GUILayout.Button("Etc")) { }
                GUI.color = normalCol;

                break;

            }

        }

        #endregion

    }

}
