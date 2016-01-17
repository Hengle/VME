using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorSearchGroup : BaseEditorGroup {

    private class FilteredNameObject {

        public string name;
        public int index;

        public FilteredNameObject (string _name, int _index) {

            index = _index;
            name = _name;

        }

    }

    public delegate void ItemSelectedAction (int _index);
    public event ItemSelectedAction OnItemSelected;

    private List<string> names = new List<string>();
    private List<FilteredNameObject> filteredNames = new List<FilteredNameObject>();

    private int selectedIndex = 0;
    private int previousSelectedIndex = 0;
    private Vector2 scrollPosition;
    private string searchName = "";
    private string previousSearchedName;

    private int groupWidth;

    public EditorSearchGroup (int _groupWidth) {

        groupWidth = _groupWidth;

    }

    public override void DrawContent () {

        base.DrawContent();

        //If selection is changed
        if (previousSelectedIndex != selectedIndex) {

            OnItemSelected(selectedIndex);
            previousSelectedIndex = selectedIndex;

        }

        EditorGUILayout.BeginVertical(GUILayout.Width(groupWidth));
        DrawSearchBar();

        //Draw Content
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Color normalColor = GUI.color;

        if (searchName.Length != 0) {

            for (int i = 0; i < filteredNames.Count; i++) {

                if (names[selectedIndex] == filteredNames[i].name) {

                    GUI.color = Color.yellow;
                    if (GUILayout.Button(filteredNames[i].name)) { }
                    GUI.color = normalColor;

                } else {

                    if (GUILayout.Button(filteredNames[i].name)) {

                        selectedIndex = filteredNames[i].index;

                    }

                }

            }

        } else {

            for (int i = 0; i < names.Count; i++) {

                if (i == selectedIndex) {

                    GUI.color = Color.yellow;
                    if (GUILayout.Button(filteredNames[i].name)) { }
                    GUI.color = normalColor;

                } else {

                    if (GUILayout.Button(names[i])) {

                        selectedIndex = i;

                    }

                }

            }

        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }

    #region UI

    private void DrawSearchBar () {

        previousSearchedName = searchName;
        searchName = EditorGUILayout.TextField(searchName);

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

    public void ChangeSelectionByID (int _id) {

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
