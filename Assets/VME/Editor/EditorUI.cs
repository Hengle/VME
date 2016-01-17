using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace EditorUI {

   /// <summary>
   /// Fancier UI functions.
   /// </summary>
	public class Draw {

        /// <summary>
        /// Draws a Title 
        /// </summary>
		public static void TitleField (string text) {

            GUIStyle style = EditorStyles.toolbarButton;
            style.fontStyle = FontStyle.Normal;
            EditorGUILayout.LabelField(text,style);

		}

        /// <summary>
        /// Draws a Pressed in Button
        /// </summary>
        public static void PressedButton (string text) {

            GUIStyle style = EditorStyles.objectFieldThumb;
            style.fontStyle = FontStyle.Normal;
            EditorGUILayout.LabelField(text, style);

        }

	}

}

