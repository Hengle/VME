using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VME {
    /// <summary>
    /// Shows info of current selection
    /// </summary>
    public class VMESelectionPanel {

        public void Draw () {

            GameObject[] selectedObjects = Selection.gameObjects;
            GUIStyle style = EditorStyles.objectFieldThumb;
            style.fontSize = 10;
            EditorGUILayout.BeginHorizontal(style);
            if (selectedObjects.Length != 0) {

                EditorGUILayout.LabelField("" + selectedObjects[selectedObjects.Length - 1].name);
                EditorGUILayout.LabelField("Total : " + selectedObjects.Length);

            }
            else {

                EditorGUILayout.LabelField("No Selection");

            }

            EditorGUILayout.EndHorizontal();
        }

    }
}