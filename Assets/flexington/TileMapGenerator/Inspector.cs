// using UnityEngine;
// using UnityEditor;
// using UnityEditorInternal;
// using flexington.TileMapGenerator;
// using System;

// [CustomEditor(typeof(flexington.TileMapGenerator.Generator))]
// public class Inspector : Editor
// {
// PreviewRenderUtility _renderer;

//     private ReorderableList _list;

//     private Generator Generator
//     {
//         get { return target as Generator; }
//     }

//     private bool _autoBalanceThreshold;

//     private bool _itemCountChanged;

//     private GUIStyle _foldoutStyle;

//     private bool _mapGroup;

//     /// <summary>
//     /// Initialize ReorderableList when Inspector is initialized
//     /// </summary>
//     private void Awake()
//     {
//         if (_list == null) _list = new ReorderableList(Generator.Tiles, typeof(Tile), true, true, true, true);
//     }

//     /// <summary>
//     /// Is called, every time when the inspector get activated focused
//     /// </summary>
//     private void OnEnable()
//     {
//         if (_list == null) _list = new ReorderableList(Generator.Tiles, typeof(Tile), true, true, true, true);

//         _list.drawHeaderCallback += DrawHeader;
//         _list.drawElementCallback += DrawElement;

//         _list.onAddCallback += AddItem;
//         _list.onRemoveCallback += RemoveItem;
//     }

//     /// <summary>
//     /// Is called when the Inspector get deactivated the focus
//     /// </summary>
//     private void OnDisable()
//     {
//         _list.drawHeaderCallback -= DrawHeader;
//         _list.drawElementCallback -= DrawElement;

//         _list.onAddCallback -= AddItem;
//         _list.onRemoveCallback -= RemoveItem;
//     }

//     /// <summary>
//     /// Drawes the header of the list
//     /// </summary>
//     /// <param name="rect"></param>
//     private void DrawHeader(Rect rect)
//     {
//         float columns = 3;
//         float margin = 5;
//         float offset = 15;
//         rect.width = ((rect.width - offset) - (columns - 1) * margin) / columns;
//         rect.x += offset;

//         GUI.Label(rect, "Activation Start");
//         rect.x += rect.width + margin;

//         GUI.Label(rect, "Activation End");
//         rect.x += rect.width + margin;

//         GUI.Label(rect, "Prefab");
//     }

//     /// <summary>
//     /// Draws one element of the list
//     /// </summary>
//     /// <param name="rect"></param>
//     /// <param name="index"></param>
//     /// <param name="isActive"></param>
//     /// <param name="isFocused"></param>
//     private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
//     {
//         flexington.TileMapGenerator.TileEntity tile = Generator.Tiles[index];

//         float columns = 3;
//         float margin = 5;
//         rect.width = (rect.width - (columns - 1) * margin) / columns;

//         EditorGUI.BeginChangeCheck();

//         tile.StartActivation = EditorGUI.FloatField(rect, tile.StartActivation);
//         rect.x += rect.width + margin;

//         tile.EndActivation = EditorGUI.FloatField(rect, tile.EndActivation);
//         rect.x += rect.width + margin;

//         tile.Prefab = EditorGUI.ObjectField(rect, tile.Prefab, typeof(GameObject), true) as GameObject;

//         if (EditorGUI.EndChangeCheck())
//         {
//             EditorUtility.SetDirty(target);
//             ValidatePrefab();
//         }
//     }

//     /// <summary>
//     /// Adds a new element to the list
//     /// </summary>
//     /// <param name="list"></param>
//     private void AddItem(ReorderableList list)
//     {
//         Generator.Tiles.Add(new flexington.TileMapGenerator.TileEntity());
//         EditorUtility.SetDirty(target);
//         _itemCountChanged = true;
//     }

//     /// <summary>
//     /// Removes the selected element from the list
//     /// </summary>
//     /// <param name="list"></param>
//     private void RemoveItem(ReorderableList list)
//     {
//         Generator.Tiles.RemoveAt(list.index);
//         EditorUtility.SetDirty(target);
//         _itemCountChanged = true;
//     }

//     /// <summary>
//     /// Outputs a warning if the list uses the same prefab more than once
//     /// </summary>
//     private void ValidatePrefab()
//     {
//         for (int i = 0; i < Generator.Tiles.Count; i++)
//         {
//             for (int j = 0; j < Generator.Tiles.Count; j++)
//             {
//                 if (i == j) continue;
//                 if (Generator.Tiles[i].Prefab == null || Generator.Tiles[j].Prefab == null) continue;
//                 if (Generator.Tiles[i].Prefab == Generator.Tiles[j].Prefab)
//                 {
//                     Debug.Log("Duplicate prefab");
//                     return;
//                 }
//             }
//         }

//     }

//     private void BalanceThreshold(bool force = false)
//     {
//         if ((!_autoBalanceThreshold || !_itemCountChanged) && !force) return;
//         _itemCountChanged = false;

//         float count = Generator.Tiles.Count;
//         float threshold = 1f / count;

//         for (int i = 0; i < count; i++)
//         {
//             Generator.Tiles[i].StartActivation = threshold * i;
//             Generator.Tiles[i].EndActivation = threshold * (i + 1);
//         }
//     }

//     private void ResetThreshold()
//     {
//         for (int i = 0; i < Generator.Tiles.Count; i++)
//         {
//             Generator.Tiles[i].StartActivation = 0;
//             Generator.Tiles[i].EndActivation = 0;
//         }
//     }

//     public override void OnInspectorGUI()
//     {
//         if (_foldoutStyle == null)
//         {
//             _foldoutStyle = new GUIStyle(EditorStyles.foldout)
//             {
//                 fontStyle = FontStyle.Bold
//             };
//         }

//         DrawMapGroup();

//         // base.OnInspectorGUI();

//     }

//     private void DrawMapGroup()
//     {
//         _mapGroup = EditorGUILayout.Foldout(_mapGroup, "Map", _foldoutStyle);

//         if (_mapGroup)
//         {
//             EditorGUILayout.BeginHorizontal();

//             EditorGUILayout.BeginVertical();
//             GUILayout.Label("Auto Balance Threshold");
//             GUILayout.Label("Map Size");
//             EditorGUILayout.EndVertical();

//             EditorGUILayout.BeginVertical();
//             _autoBalanceThreshold = EditorGUILayout.Toggle(_autoBalanceThreshold);
//             Generator.MapSize = EditorGUILayout.Vector2IntField("", Generator.MapSize);
//             EditorGUILayout.EndVertical();

//             EditorGUILayout.EndHorizontal();

//             EditorGUILayout.Space();

//             _list.DoLayoutList();
//             ValidatePrefab();
//             BalanceThreshold();

//             EditorGUILayout.Space();

//             EditorGUILayout.BeginHorizontal();

//             EditorGUILayout.BeginVertical();
//             if (GUILayout.Button("Generate Map")) { Generator.GenerateMap(); }
//             if (GUILayout.Button("Generate Threshold")) { BalanceThreshold(true); }
//             EditorGUILayout.EndVertical();

//             EditorGUILayout.BeginVertical();
//             if (GUILayout.Button("Reset Map")) { Generator.ResetMap(); }
//             if (GUILayout.Button("Reset Threshold")) { ResetThreshold(); }
//             EditorGUILayout.EndVertical();

//             EditorGUILayout.EndHorizontal();
//         }
//     }
// }