using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

namespace VME {

    /// <summary>
    /// Voxel Map Main Panel.
    /// 
    /// Used for handling and drawing Main related features.
    /// </summary>
    public class VMEMainPanel : BaseEditorPanel {

        public VMEMainPanel (EditorWindow _window, KeyCode _toggleKey) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;

        }

        /// <summary>
        /// Handles snap function and panel drawing.
        /// </summary>
        public VMESnapPanel snapPanel = new VMESnapPanel();

        #region BaseEditorWindow Functions

        /// <summary>
        /// See 
        /// </summary>
        public override void DrawContent () {

            base.DrawContent();

            snapPanel.Draw();

        }

        /// <summary>
        /// See BaseEditorPanel.Input.
        /// </summary>
        public override void Input (SceneView sceneView) {

            base.Input(sceneView);
            snapPanel.Input();

        }

        #endregion

    }

}

