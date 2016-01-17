using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

namespace VME.Settings {
    public class VMESettingsKeyPanel : BaseEditorPanel {
 

        public VMESettingsKeyPanel (VMESettingsObject _object) {

            settingsObject = _object;

        }

        public override void DrawContent () {

            base.DrawContent();

            EditorGUILayout.Space();
            EditorUI.Draw.TitleField("Main");
            settingsObject.TOGGLE_EDITOR = DrawKeyCodeField("Editor Toggle",settingsObject.TOGGLE_EDITOR);

            EditorGUILayout.Space();
            EditorUI.Draw.TitleField("Actions");
            settingsObject.APPLY_SINGLE = DrawKeyCodeField("Apply to: Single", settingsObject.APPLY_SINGLE);
            settingsObject.APPLY_ALL = DrawKeyCodeField("Apply to: All", settingsObject.APPLY_ALL);
            settingsObject.ROTATE_TARGET = DrawKeyCodeField("Rotate Target", settingsObject.ROTATE_TARGET);
            settingsObject.CHANGEBLOCK_STYLE = DrawKeyCodeField("Change Style", settingsObject.CHANGEBLOCK_STYLE);
            settingsObject.PICK_TILE = DrawKeyCodeField("Tile Picker", settingsObject.PICK_TILE);

            EditorGUILayout.Space();
            EditorUI.Draw.TitleField("Panel Toggles");
            settingsObject.ENABLE_FILEMANAGER = DrawKeyCodeField("Toggle FileManager Panel", settingsObject.ENABLE_FILEMANAGER);
            settingsObject.ENABLE_MODE = DrawKeyCodeField("Toggle Mode Panel", settingsObject.ENABLE_MODE);
            settingsObject.ENABLE_SNAP = DrawKeyCodeField("Toggle Snap Panel", settingsObject.ENABLE_SNAP);
            settingsObject.ENABLE_SWATCH = DrawKeyCodeField("Toggle Swatch Panel", settingsObject.ENABLE_SWATCH);
            settingsObject.ENABLE_CHUNK = DrawKeyCodeField("Toggle Chunk Panel", settingsObject.ENABLE_CHUNK);

            EditorGUILayout.Space();
            EditorUI.Draw.TitleField("Mode Selection");
            settingsObject.SET_MODE_TO_SELECT = DrawKeyCodeField("Set to Selection Mode", settingsObject.SET_MODE_TO_SELECT);
            settingsObject.SET_MODE_TO_MOVE = DrawKeyCodeField("Set to Move Mode", settingsObject.SET_MODE_TO_MOVE);
            settingsObject.SET_MODE_TO_PAINT = DrawKeyCodeField("Set to Paint Mode", settingsObject.SET_MODE_TO_PAINT);
            settingsObject.SET_MODE_TO_EDIT = DrawKeyCodeField("Set to Edit Mode", settingsObject.SET_MODE_TO_EDIT);
            settingsObject.SET_MODE_TO_REMOVE = DrawKeyCodeField("Set to Remove Mode", settingsObject.SET_MODE_TO_REMOVE);

            EditorGUILayout.Space();
            EditorUI.Draw.TitleField("Swatch");
            settingsObject.SWATCH_ENABLE_SEARCH = DrawKeyCodeField("Enable Search", settingsObject.SWATCH_ENABLE_SEARCH);
            settingsObject.SWATCH_SELECT_CATEGORY_DECREASE = DrawKeyCodeField("Previous Category", settingsObject.SWATCH_SELECT_CATEGORY_DECREASE);
            settingsObject.SWATCH_SELECT_CATEGORY_INCREASE = DrawKeyCodeField("Next Category", settingsObject.SWATCH_SELECT_CATEGORY_INCREASE);
            settingsObject.SWATCH_ITEM_DECREASE = DrawKeyCodeField("Previous Item", settingsObject.SWATCH_ITEM_DECREASE);
            settingsObject.SWATCH_ITEM_INCREASE = DrawKeyCodeField("Next Item", settingsObject.SWATCH_ITEM_INCREASE);

        }

        private KeyCode DrawKeyCodeField(string _labelName,KeyCode key) {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(_labelName);
            key = (KeyCode)EditorGUILayout.EnumPopup(key);

            GUILayout.EndHorizontal();
            return key;
        }

        #region Functions

        #endregion

    }
}

