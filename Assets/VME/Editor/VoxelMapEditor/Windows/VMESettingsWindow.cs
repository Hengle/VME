using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;

namespace VME.Settings {

    public class VMESettingsWindow : EditorWindow {

        private bool resetToggled = false;
        private static VMESettingsObject settingsObject;
        private static VMESettingsKeyPanel keyPanel;

        [MenuItem("VME/Open/Settings")]
        [DidReloadScripts]
        static void Init () {

            VMESettingsWindow window = (VMESettingsWindow)EditorWindow.GetWindow(typeof(VMESettingsWindow));

            window.Show();
            window.titleContent.text = "VME Settings";
            settingsObject = VMESettingsObject.LoadScriptableObject();
            keyPanel = new VMESettingsKeyPanel(settingsObject);

        }

        void OnGUI () {

            DrawResetFactoryUI();
            keyPanel.Draw("Input");

        }

        #region UI

        private void DrawResetFactoryUI () {

            if (GUILayout.Button("Reset Factory Settings")) {

                resetToggled = !resetToggled;

            }

            if (resetToggled) {

                if (GUILayout.Button("Are you sure?")) {

                    settingsObject.RestoreToFactorySettings();
                    resetToggled = false;

                }

            }

        }

        #endregion

    }

}
