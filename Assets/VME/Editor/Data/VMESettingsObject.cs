using UnityEngine;
using System.Collections;

/// <summary>
/// Holds all personal settings for the editor in a object inside the editor project.
/// </summary>
public class VMESettingsObject : ScriptableObject {

    //Key codes
    public KeyCode TOGGLE_EDITOR;
    public KeyCode SHOW_GRID;

    //Panel Toggles
    public KeyCode ENABLE_FILEMANAGER;
    public KeyCode ENABLE_SWATCH;
    public KeyCode ENABLE_SNAP;
    public KeyCode ENABLE_MODE;
    public KeyCode ENABLE_CHUNK;

    //Mode Selection
    public KeyCode SET_MODE_TO_SELECT;
    public KeyCode SET_MODE_TO_MOVE;
    public KeyCode SET_MODE_TO_PAINT;
    public KeyCode SET_MODE_TO_EDIT;
    public KeyCode SET_MODE_TO_REMOVE;

    //Modify Keys
    public KeyCode APPLY_SINGLE;
    public KeyCode APPLY_ALL;
    public KeyCode ROTATE_TARGET;
    public KeyCode CHANGEBLOCK_STYLE;
    public KeyCode PICK_TILE;

    //Swatch
    public KeyCode SWATCH_ENABLE_SEARCH;
    public KeyCode SWATCH_SELECT_CATEGORY_DECREASE;
    public KeyCode SWATCH_SELECT_CATEGORY_INCREASE;
    public KeyCode SWATCH_ITEM_INCREASE;
    public KeyCode SWATCH_ITEM_DECREASE;

    /// <summary>
    /// Gets the file located in the Project.
    /// </summary>
    /// <returns>The ScriptableObject of type VMESettingsObject. </returns>
    public static VMESettingsObject LoadScriptableObject() {
       
        return (VMESettingsObject)Resources.Load("VME_Data/VMESettings");

    }

    /// <summary>
    /// Restores all settings to the default set by the creator.
    /// </summary>
    public void RestoreToFactorySettings () {

        APPLY_ALL = KeyCode.H;
        APPLY_SINGLE = KeyCode.G;
        ROTATE_TARGET = KeyCode.X;
        CHANGEBLOCK_STYLE = KeyCode.V;
        PICK_TILE = KeyCode.I;

        ENABLE_CHUNK = KeyCode.F10;
        ENABLE_FILEMANAGER = KeyCode.F12;
        ENABLE_SWATCH = KeyCode.F9;
        ENABLE_MODE = KeyCode.F1;

        SET_MODE_TO_SELECT = KeyCode.Alpha1;
        SET_MODE_TO_MOVE = KeyCode.Alpha2;
        SET_MODE_TO_EDIT = KeyCode.Alpha3;
        SET_MODE_TO_PAINT = KeyCode.Alpha4;
        SET_MODE_TO_REMOVE = KeyCode.Alpha5;

        SHOW_GRID = KeyCode.F5;

        SWATCH_ENABLE_SEARCH = KeyCode.F2;
        SWATCH_ITEM_DECREASE = KeyCode.Keypad8;
        SWATCH_ITEM_INCREASE = KeyCode.Keypad2;
        SWATCH_SELECT_CATEGORY_DECREASE = KeyCode.Keypad4;
        SWATCH_SELECT_CATEGORY_INCREASE = KeyCode.Keypad6;

    }
}
