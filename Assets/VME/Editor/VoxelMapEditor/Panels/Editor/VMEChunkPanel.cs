using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorUI;
using System.Collections.Generic;

namespace VME {

    /// <summary>
    /// Panel used for the Chunks in the VME Editor.
    /// </summary>
    public class VMEChunkPanel : BaseEditorPanel {

        /// <summary>
        /// Constructor.
        /// </summary>
        public VMEChunkPanel (EditorWindow _window, KeyCode _toggleKey) {

            base.toggleKey = _toggleKey;
            base.parentWindow = _window;

        }

        /// <summary>
        /// Reference of the voxel map that is used in the editor.
        /// </summary>
        private VoxelMap map;

        /// <summary>
        /// List of all chunks in the voxel map.
        /// </summary>
        private List<ChunkSpawner> chunks = new List<ChunkSpawner>();

        /// <summary>
        /// Index of selectedChunk.
        /// </summary>
        private int selectedChunkIndex = 0;

        /// <summary>
        /// Current scroll position of the panel.
        /// </summary>
        private Vector2 scrollPosition;

        /// <summary>
        /// Current object being selected, used for checking if its a chunk.
        /// </summary>
        private GameObject selectedObject;

        /// <summary>
        /// The name being used if you create a new chunk.
        /// </summary>
        private string newChunkName;
 
        /// <summary>
        /// Called when the panel is opened.
        /// </summary>
        public override void OnOpened () {

            base.OnOpened();

            RebuildChunkList();

        }
        
        /// <summary>
        /// Draws the content of the panel.
        /// </summary>
        public override void DrawContent () {

            base.DrawContent();

            if(map != null) {

                if (selectedObject != Selection.activeGameObject) {

                    OnSelectionChange();

                }

                DrawChunksPanel();
                DrawChunkCreatePanel();

            } else {

                EditorGUILayout.HelpBox("No VoxelSwatch Loaded. Import a VoxelMap in the FileManager Panel", MessageType.Error);

            }

        }

        #region UI

        /// <summary>
        /// Draws the panel for creating a new chunk create panel.
        /// </summary>
        private void DrawChunkCreatePanel () {

            EditorGUILayout.BeginVertical();

            newChunkName = EditorGUILayout.TextField(newChunkName);

            if(GUILayout.Button("Create New ChunkSpawner")) {

                CreateChunk(newChunkName);

            }

            EditorGUILayout.EndVertical();

        }

        /// <summary>
        /// Draws the panel for showing all chunks.
        /// </summary>
        private void DrawChunksPanel () {

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(90));

            
            for(int i = 0; i < chunks.Count; i++) {

                EditorGUILayout.BeginHorizontal();

                chunks[i].name = EditorGUILayout.TextField(chunks[i].name, GUILayout.Width(10));

                if (selectedChunkIndex == i) {

                    EditorUI.Draw.PressedButton(chunks[i].name);

                } else {

                    if (GUILayout.Button(chunks[i].name)) {
                        
                        Selection.activeGameObject = chunks[i].gameObject;
                        SceneView.lastActiveSceneView.FrameSelected();
                        selectedChunkIndex = i;

                    }

                }

                if (GUILayout.Button("Save", GUILayout.Width(50))) {

                    if (chunks[i].chunk != null) {

                        chunks[i].SaveInProject(map.name);

                    } else {

                        Debug.LogWarning("No Chunk is loaded, so nothing is saved");

                    }

                }

                if (GUILayout.Button("Load", GUILayout.Width(50))) {

                    chunks[i].LoadFromProject(map.name);

                }

                if (GUILayout.Button("Delete", GUILayout.Width(50))) {

                    Object.DestroyImmediate(chunks[i].gameObject);
                    RebuildChunkList();

                }

                EditorGUILayout.EndHorizontal();

            }
           
            EditorGUILayout.EndScrollView();

        }

        #endregion

        #region Functions

        /// <summary>
        /// When selection is changed.
        /// </summary>
        private void OnSelectionChange () {

            for (int i = 0; i < chunks.Count; i++) {

                if(chunks[i].gameObject == Selection.activeGameObject) {

                    FocusChunk(i);

                }

            }

            selectedObject = Selection.activeGameObject;

        }

        /// <summary>
        /// Focus on chunk.
        /// </summary>
        /// <param name="_index">index in chunk list.</param>
        private void FocusChunk (int _index) {

            Selection.activeGameObject = chunks[_index].gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
            selectedChunkIndex = _index;

        }

        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="name">name of the new chunk.</param>
        private void CreateChunk (string name) {

            GameObject spawner = new GameObject(name);
            spawner.transform.parent = map.transform;
            ChunkSpawner chunkspawner = spawner.AddComponent<ChunkSpawner>();
            chunks.Add(chunkspawner);

            newChunkName = "";
            RebuildChunkList();
            selectedChunkIndex = GetIndexOfChunk(chunkspawner);
            FocusChunk(selectedChunkIndex);

        }

        /// <summary>
        /// Changes the voxel map reference.
        /// </summary>
        /// <param name="_map">the new reference.</param>
        public void SetVoxelMap(VoxelMap _map) {

            map = _map;
            RebuildChunkList();

        }

        /// <summary>
        /// Rebuilds the list of chunks.
        /// </summary>
        private void RebuildChunkList () {

            chunks.Clear();

            if (map != null) {

                ChunkSpawner[] chunksInMap = map.GetComponentsInChildren<ChunkSpawner>();

                for (int i = 0; i < chunksInMap.Length; i++) {

                    chunks.Add(chunksInMap[i]);

                }

            } else {

                Debug.LogWarning("Tried to open Window, but theres no VoxelMap assigned in the FileManager.");

            }

        }

        /// <summary>
        /// Gets the index of the chunk that we want.
        /// </summary>
        /// <param name="_spawner">the spawner we want the index from.</param>
        /// <returns>The index of the chunk.S</returns>
        private int GetIndexOfChunk(ChunkSpawner _spawner) {

            for (int i = 0; i < chunks.Count; i++) {

                if(chunks[i] == _spawner) {

                    return i;

                }

            }

            return 0;

        }

        #endregion

    }

}