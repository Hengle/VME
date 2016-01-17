using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;

/// <summary>
/// BaseClass for a Editor Group. same as Panel but without a toggle state
/// </summary>
public class BaseEditorGroup {

    /// <summary>
    /// The Editor Window this panel is in.
    /// </summary>
    public EditorWindow parentWindow;

    /// <summary>
    /// Reference to the Object that holds the editor settings.
    /// </summary>
    public VMESettingsObject settingsObject;

    /// <summary>
    /// Draws the panel itself, not it's content.
    /// </summary>
    public void Draw () {

        DrawContent();

    }

    /// <summary>
    /// Draws the content of the panel.
    /// </summary>
    public virtual void DrawContent () { }

    /// <summary>
    /// Used for drawing in the scene view.
    /// </summary>
    public virtual void OnSceneUI (SceneView _sceneView) { }

    /// <summary>
    /// Used for handling keyboard input.
    /// </summary>
    public virtual void Input (SceneView _sceneView) {

        if (settingsObject == null) {

            settingsObject = VMESettingsObject.LoadScriptableObject();

        }

        OnSceneUI(_sceneView);

    }

    #region functions

   
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

}
