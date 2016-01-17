using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorSearchWindow : BaseEditorPanel {

    private class FilteredNameObject {

        public string name;
        public int index;

        public FilteredNameObject(string _name,int _index) {

            index = _index;
            name = _name;

        }

    }

    public delegate void ItemSelectedAction (int _index);
    public event ItemSelectedAction OnItemSelected;

    private List<string> names = new List<string>();
    private List<FilteredNameObject> filteredNames = new List<FilteredNameObject>();

    private KeyCode increaseKey;
    private KeyCode decreaseKey;

    private int selectedIndex = 0;
    private int previousSelectedIndex = 0;
    private Vector2 scrollPosition;
    private string searchName = "";
    private string previousSearchedName;
    private bool goFocus;


    public EditorSearchWindow (KeyCode _toggleKey,KeyCode _increasekey,KeyCode _decreasekey,EditorWindow _window) {

        base.toggleKey = _toggleKey;
        increaseKey = _increasekey;
        decreaseKey = _decreasekey;
        parentWindow = _window;

    }

    public override void DrawContent () {

        base.DrawContent();

        //If selection is changed
        if(previousSelectedIndex != selectedIndex) {

            OnItemSelected(selectedIndex);
            previousSelectedIndex = selectedIndex;

        }

        DrawSearchBar();

        //Draw Content
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        if (searchName.Length != 0) {

            for (int i = 0; i < filteredNames.Count; i++) {

                if (names[selectedIndex] == filteredNames[i].name) {

                    EditorUI.Draw.PressedButton(names[selectedIndex]);

                } else {

                    if (GUILayout.Button(filteredNames[i].name)) {

                        selectedIndex = filteredNames[i].index;

                    }

                }

            }
 
        } else {

            for (int i = 0; i < names.Count; i++) {

                if (i == selectedIndex) {

                    EditorUI.Draw.PressedButton(names[i]);

                } else {

                    if (GUILayout.Button(names[i])) {

                        selectedIndex = i;

                    }

                }

            }

        }

        EditorGUILayout.EndScrollView();

        //Listen for Input while window is selected

        EditorInput();

    }

    private void EditorInput () {
        Event e = Event.current;

        if (e.isKey) {

            if (e.type == EventType.KeyDown) {

                if (e.keyCode == decreaseKey) {
       
                    ChangeSelectionFromSortedList(-1);

                }
                if (e.keyCode == increaseKey) {

                    ChangeSelectionFromSortedList(1);

                }
               
            }

        }

    }

    public override void Input (SceneView sceneView) {

        bool previousState = base.isOpen;
        base.Input(sceneView);
 
        if (base.isOpen != previousState) {

            if (base.isOpen) {

                parentWindow.Focus();
                goFocus = true;

            }

        }

    }

    #region UI

    private void DrawSearchBar () {

        previousSearchedName = searchName;
        GUI.SetNextControlName("SearchBar");
        searchName = EditorGUILayout.TextField(searchName);

        //Refocus window if input is received
        if (base.isOpen && goFocus) {
            
            EditorGUI.FocusTextInControl("SearchBar");
            goFocus = false;

        }

        if (previousSearchedName != searchName) {

            OnSearchChanged();

        }

    }

    void OnSearchChanged () {

        RebuildList();

    }

    #endregion

    #region Functions

    public List<string> GetCurrentListOfItems () {
        return names;
    }

    public void ChangeSelectionByID(int _id) {
        selectedIndex = _id;

        OnItemSelected(selectedIndex);
        previousSelectedIndex = selectedIndex;

    }

    private void ChangeSelectionFromSortedList (int increment) {

        int foundIndex = -2;

        for (int i = 0; i < filteredNames.Count; i++) {

            if (names[selectedIndex] == filteredNames[i].name) {

                foundIndex = i;

            }

        }
        
        if (foundIndex != -2) {

            if ((foundIndex + increment) != -1 && foundIndex + increment != filteredNames.Count) {

                selectedIndex = filteredNames[foundIndex + increment].index;

            }

        } else {

            selectedIndex = filteredNames[0].index;
           
        }

    }

    private void RebuildList () {

        filteredNames.Clear();

        for (int i = 0; i < names.Count; i++) {


            if (names[i].Contains(searchName) || names[i].ToLower().Contains(searchName.ToLower())) {

                FilteredNameObject obj = new FilteredNameObject(names[i], i);
              
                filteredNames.Add(obj);

            }

        }
    }

    public void SetContent (List<string> _names) {

        names.Clear();
        names = _names;
        RebuildList();
        selectedIndex = 0;
        OnItemSelected(selectedIndex);

    }

    public int GetSelectedIndex () {

        return selectedIndex;

    }

    #endregion

}
