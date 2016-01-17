using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Threading;

namespace VME {

    /// <summary>
    /// Handles the controls for selecting tiles.
    /// </summary>
    public class VMESelectionControls {

        /// <summary>
        /// Constructor.
        /// </summary>
        public VMESelectionControls () {

            //Get reference to settings
            settings = VMESettingsObject.LoadScriptableObject();

            //See if theres a target object in the scene, if not -> add it.
            firstTarget = GameObject.Find("VME_FirstTarget");

            if (firstTarget == null) {

                firstTarget = new GameObject("VME_FirstTarget");

            }

            firstTarget.hideFlags = HideFlags.HideInHierarchy;

            //Same for second.
            secondTarget = GameObject.Find("VME_SecondTarget");

            if (secondTarget == null) {

                secondTarget = new GameObject("VME_SecondTarget");

            }

            secondTarget.hideFlags = HideFlags.HideInHierarchy;

        }

        /// <summary>
        /// The first position that is clicked.
        /// </summary>
        private GameObject firstTarget;

        /// <summary>
        /// The second position that is clicked.
        /// </summary>
        private GameObject secondTarget;

        /// <summary>
        /// State of where we are in selecting.
        /// </summary>
        private enum SelectState {
            None,
            One,
            Two
        }

        /// <summary>
        /// The current state we are in.
        /// </summary>
        private SelectState state = SelectState.None;

        /// <summary>
        /// Reference to the settings of the editor.
        /// </summary>
        private VMESettingsObject settings;

        /// <summary>
        /// Handles editor input.
        /// </summary>
        public void Input (SceneView view) {

            Event e = Event.current;

            if (e.isKey) {

                if (e.type == EventType.KeyDown) {

                    if (e.keyCode == settings.APPLY_SINGLE) {

                        if (state == SelectState.Two) {

                            state = SelectState.None;
                            GetSelectionOfObjects();

                        }

                    }

                }

            }

            if (e.type == EventType.MouseDown) {

                Vector3 tar = VMEGlobal.GetPositionNextToHoveredTile();
    
                if (e.button == 0) {

                    if (tar == new Vector3(0, 9000, 0)) {

                        firstTarget.transform.position = new Vector3(0, 9000, 0);
                        secondTarget.transform.position = new Vector3(0, 9000, 0);
                        state = SelectState.None;

                    } else {

                        if (state == SelectState.None) {

                            firstTarget.transform.position = tar;
                            state = SelectState.One;

                        } else if (state == SelectState.One) {

                            secondTarget.transform.position = tar;
                            state = SelectState.Two;

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Get the tiles within the two placed selection points.
        /// </summary>
        private void GetSelectionOfObjects () {

            List<GameObject> hits = new List<GameObject>();

            Vector3 p1 = firstTarget.transform.position;
            Vector3 p2 = secondTarget.transform.position;

            Vector3 scale = p1 - p2;
            scale.x = Mathf.Abs(scale.x);
            scale.y = Mathf.Abs(scale.y);
            scale.z = Mathf.Abs(scale.z);

            RaycastHit[] check = Physics.BoxCastAll((p1 + p2) * 0.5f, scale * 0.5f, Vector3.up);

            for (int i = 0; i < check.Length; i++) {

                hits.Add(check[i].collider.gameObject);

            }

            Selection.objects = hits.ToArray();

        }

    }

}