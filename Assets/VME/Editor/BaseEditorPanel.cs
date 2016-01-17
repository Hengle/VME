using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

/// <summary>
/// BaseClass for a Editor Panel
/// </summary>
public class BaseEditorPanel {

    /// <summary>
    /// if the panel is currently opened.
    /// </summary>
    public bool isOpen = false;

    /// <summary>
    /// Key used for activating this panel.
    /// </summary>
    public KeyCode toggleKey;

    /// <summary>
    /// The Editor Window this panel is in.
    /// </summary>
    public EditorWindow parentWindow;

    /// <summary>
    /// Reference to the Object that holds the editor settings.
    /// </summary>
    public VMESettingsObject settingsObject;

    /// <summary>
    /// what the previous state was.
    /// </summary>
    private bool previousOpenState = false;

    /// <summary>
    /// Draws the panel itself, not it's content.
    /// </summary>
    public void Draw (string _name) {

        GUIStyle style = EditorStyles.objectFieldThumb;
        style.fontSize = 10;

        EditorGUILayout.BeginVertical(style);

        EditorGUILayout.BeginHorizontal();

            EditorUI.Draw.TitleField(_name);
            isOpen = EditorGUILayout.Toggle(isOpen, new GUILayoutOption[] { GUILayout.Width(20)});

        EditorGUILayout.EndHorizontal();

        if (isOpen) {

           DrawContent();

        }

        //End Window
        EditorGUILayout.EndVertical();

        CheckPanelState();

    }

    /// <summary>
    /// Draws the content of the panel.
    /// </summary>
    public virtual void DrawContent () {}

    /// <summary>
    /// Called when the panel is opened.
    /// </summary>
    public virtual void OnOpened () {}

    /// <summary>
    /// Called when the panel is closed.
    /// </summary>
    public virtual void OnClosed () {}

    /// <summary>
    /// Used for drawing in the scene view.
    /// </summary>
    public virtual void OnSceneUI (SceneView _sceneView) {}

    /// <summary>
    /// Used for handling keyboard input.
    /// </summary>
    public virtual void Input (SceneView _sceneView) {

        if (settingsObject == null) {
            
            settingsObject = VMESettingsObject.LoadScriptableObject();

        }

        Event e = Event.current;
  
        if (e.isKey) {

            if (e.type == EventType.KeyDown) {

                if (e.keyCode == toggleKey) {
            
                    isOpen = !isOpen;

                }

            }

        }

        OnSceneUI(_sceneView);

    }

    #region functions

    /// <summary>
    /// Checks if the window state is changed.
    /// </summary>
    public void CheckPanelState () {

        if (isOpen != previousOpenState) {

            if (isOpen) {

                OnOpened();

            } else {

                OnClosed();

            }

            previousOpenState = isOpen;

        }

    }

    #endregion

    #region EventListeners

    /// <summary>
    /// Adds the events used in this editor panel.
    /// </summary>
    public virtual void AddEventListeners (SceneView _sceneview) {

        SceneView.onSceneGUIDelegate += Input;

    }

    /// <summary>
    /// Removes the events used in this editor panel.
    /// </summary>
    public virtual void RemoveEventlisteners (SceneView _sceneview) {

        SceneView.onSceneGUIDelegate -= Input;

    }

    #endregion

    /// <summary>
    /// Sets the current state of the panel.
    /// </summary>
    public void SetState(bool _state) {

        isOpen = _state;
        CheckPanelState();

    }

}
