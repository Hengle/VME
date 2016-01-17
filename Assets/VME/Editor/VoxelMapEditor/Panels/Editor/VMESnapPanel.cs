using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

namespace VME {

    /// <summary>
    /// Voxel Map Snap Panel
    /// 
    /// Used for handling and drawing Snap related features.
    /// </summary>
    public class VMESnapPanel {

        /// <summary>
        /// If position snap is enabled.
        /// </summary>
        public bool positionSnapIsActive;

        /// <summary>
        /// If  snap is enabled.
        /// </summary>
        public bool rotationSnapIsActive;

        /// <summary>
        /// Types of position snap.
        /// </summary>
        private enum PositionSnapType {

            FullBlock,
            HalfBlock,
            QuarterBlock,
            PerVoxel,
            Freeform

        }

        /// <summary>
        /// Types of rotation snap.
        /// </summary>
        private enum RotationSnapType {
            Degrees90,
            Degrees45,
            Freeform
        }

        /// <summary>
        /// current PositionSnapTye.
        /// </summary>
        private PositionSnapType positionSnapType = PositionSnapType.FullBlock;

        /// <summary>
        /// current PositionSnapTye.
        /// </summary>
        private RotationSnapType rotationSnapType = RotationSnapType.Degrees90;

        //----Constant values---------------------->

        private const float FULL_BLOCK = 1.0f;
        private const float HALF_BLOCK = 0.5f;
        private const float QUARTER_BLOCK = 0.25f;
        private const float PER_VOXEL = 0.0625f;

        private const float Degrees90 = 90.0f;
        private const float Degrees45 = 45.0f;

        //----------------------------------------->

        /// <summary>
        /// Draws the snap panel.
        /// </summary>
        public void Draw () {

            //Draw position snap.
            EditorGUILayout.BeginHorizontal();
            positionSnapIsActive = EditorGUILayout.Toggle(positionSnapIsActive, new GUILayoutOption[] { GUILayout.Width(20)});
            positionSnapType = (PositionSnapType)EditorGUILayout.EnumPopup(positionSnapType);
            EditorGUILayout.EndHorizontal();

            //Draw rotation snap.
            EditorGUILayout.BeginHorizontal();
            rotationSnapIsActive = EditorGUILayout.Toggle(rotationSnapIsActive, new GUILayoutOption[] { GUILayout.Width(20) });
            rotationSnapType = (RotationSnapType)EditorGUILayout.EnumPopup(rotationSnapType);
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// Handles input for this VMESnapPanel.
        /// </summary>
        public void Input () {

            Event e = Event.current;
           
            if (e.type == EventType.MouseUp) {

                //Handling position snap.
                if (positionSnapIsActive) {

                    float factor = GetPositionSnapFactor();

                    GameObject[] selections = Selection.gameObjects;

                    for (int i = 0; i < selections.Length; i++) {

                        SnapTransformToPostition(selections[i].transform, factor);

                    }

                }

                //handling rotation snap.
                if (rotationSnapIsActive) {

                    float factor = GetRotationSnapFactor();

                    GameObject[] selections = Selection.gameObjects;

                    for (int i = 0; i < selections.Length; i++) {

                        RotationSnap(selections[i].transform, factor);

                    }

                }


            }

        }

        #region Functions

        /// <summary>
        /// Receives which PositionSnapType is currently active.
        /// </summary>
        /// <returns>The value that will be used for position snapping.</returns>
        private float GetPositionSnapFactor () {

            float factor = 0;

            switch (positionSnapType) {

                case PositionSnapType.FullBlock:
                factor = FULL_BLOCK;
                break;

                case PositionSnapType.HalfBlock:
                factor = HALF_BLOCK;
                break;

                case PositionSnapType.QuarterBlock:
                factor = QUARTER_BLOCK;
                break;

                case PositionSnapType.PerVoxel:
                factor = PER_VOXEL;
                break;

            }

            return factor;

        }

        /// <summary>
        /// Receives which RotationSnapType is currently active.
        /// </summary>
        /// <returns>The value that will be used for rotation snapping.</returns>
        private float GetRotationSnapFactor () {

            float factor = 0;

            switch (rotationSnapType) {

                case RotationSnapType.Degrees45:
                factor = Degrees45;
                break;

                case RotationSnapType.Degrees90:
                factor = Degrees90;
                break;

            }

            return factor;

        }

        /// <summary>
        /// Snaps the object into position
        /// </summary>
        /// <param name="_object">The object that will be position snapped.</param>
        /// <param name="_snapfactor">The snapFactorValue used.</param>
        private void SnapTransformToPostition(Transform _object,float _snapfactor) {

            _object.position = new Vector3(
                Mathf.Round(_object.position.x / _snapfactor) * _snapfactor,
                Mathf.Round(_object.position.y / _snapfactor) * _snapfactor,
                Mathf.Round(_object.position.z / _snapfactor) * _snapfactor
            );

        }

        /// <summary>
        /// Snaps the object into rotation.
        /// </summary>
        /// <param name="_object">The object that will be position snapped.</param>
        /// <param name="_snapfactor">The snapFactorValue used.</param>
        private void RotationSnap (Transform _object, float _snapfactor) {

            _object.transform.rotation = Quaternion.Euler(new Vector3(
                Mathf.Round(_object.rotation.eulerAngles.x / _snapfactor) * _snapfactor,
                Mathf.Round(_object.rotation.eulerAngles.y / _snapfactor) * _snapfactor,
                Mathf.Round(_object.rotation.eulerAngles.z / _snapfactor) * _snapfactor
            ));

        }

        #endregion

    }

}

