using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

namespace VME {

    public class EditorPanelTemplate : BaseEditorPanel {



        public EditorPanelTemplate (EditorWindow _window, KeyCode _toggleKey) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;

        }

        public override void DrawContent () {

            base.DrawContent();



        }

        public override void Input (SceneView sceneView) {

            base.Input(sceneView);

            Event e = Event.current;

            if (e.isKey) {

                if (e.type == EventType.KeyDown) {



                }

            }

        }

        #region UI


        #endregion

        #region Functions



        #endregion

    }

}