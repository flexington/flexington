// using UnityEngine;
// using UnityEditor;

// namespace flexington.TileMapGenerator
// {
//     public class Editor : EditorWindow
//     {
//         private GameObject _gameObject;
//         private UnityEditor.Editor _gameObjectEditor;
//         PreviewRenderUtility _renderer;

//         GUIStyle _foldoutStyle;

//         #region General
//         private bool _generalGroup;
//         private bool _useRandomSeed = true;
//         private string _seed;
//         private Vector2 _mapSize;
//         #endregion

//         #region Ground
//         private bool _groundGroup;
//         private int _fillPercent;
//         private int _smoothing;
//         #endregion

//         #region Debug
//         private bool _debugGroup;
//         private bool _drawGizmo;
//         #endregion

//         [MenuItem("flexington/3D/TileMapGenerator")]
//         private static void ShowWindow()
//         {
//             var window = GetWindow<flexington.TileMapGenerator.Editor>();
//             window.titleContent = new GUIContent("TileMapGenerator");
//             window.Show();
//         }

//         void OnGUI()
//         {
//             if (_foldoutStyle == null)
//             {
//                 _foldoutStyle = new GUIStyle(EditorStyles.foldout)
//                 {
//                     fontStyle = FontStyle.Bold
//                 };
//             }

//             DrawGeneral();
//             DrawGround();
//             DrawDebug();

//             if (GUILayout.Button("Generate Map"))
//             {

//             }


//             // EditorGUILayout.BeginHorizontal();
//             // GUILayout.Label("Test");
//             // _gameObject = (GameObject)EditorGUILayout.ObjectField(_gameObject, typeof(GameObject), true);
//             // EditorGUILayout.EndHorizontal();

//             // EditorGUILayout.BeginHorizontal();
//             // GUILayout.FlexibleSpace();
//             // GUILayout.Button("1");
//             // GUILayout.Button("2");
//             // GUILayout.Button("3");
//             // GUILayout.FlexibleSpace();
//             // EditorGUILayout.EndHorizontal();

//             // GUILayoutOption[] options = { GUILayout.ExpandWidth(false) };
//             // Rect r = GUILayoutUtility.GetRect(0.0f, 100f, 0f, 100f, options);
//             // if (_gameObject != null)
//             // {
//             //     BeginDraw(r);
//             //     DrawRenderPreview();
//             //     EndDraw(r);
//             // }
//         }

//         private void DrawGeneral()
//         {
//             _generalGroup = EditorGUILayout.Foldout(_generalGroup, "General", _foldoutStyle);
//             if (_generalGroup)
//             {

//                 // Map Size
//                 EditorGUILayout.BeginHorizontal();
//                 GUILayout.Label("Map Size");
//                 _mapSize = EditorGUILayout.Vector2Field(string.Empty, _mapSize);
//                 EditorGUILayout.EndHorizontal();

//                 // Use Random Seed
//                 EditorGUILayout.BeginHorizontal();
//                 GUILayout.Label("Use Random Seed");
//                 _useRandomSeed = EditorGUILayout.Toggle(_useRandomSeed);
//                 EditorGUILayout.EndHorizontal();

//                 // Manual Seed
//                 if (!_useRandomSeed)
//                 {
//                     EditorGUILayout.BeginHorizontal();
//                     GUILayout.Label("Seed");
//                     _seed = EditorGUILayout.TextField(_seed);
//                     EditorGUILayout.EndHorizontal();
//                 }
//                 EditorGUILayout.Space();
//             }

//         }

//         private void DrawGround()
//         {
//             _groundGroup = EditorGUILayout.Foldout(_groundGroup, "Ground", _foldoutStyle);

//             if (_groundGroup)
//             {
//                 // Fill Percent
//                 EditorGUILayout.BeginHorizontal();
//                 GUILayout.Label("Fill Percent");
//                 _fillPercent = EditorGUILayout.IntSlider(_fillPercent, 0, 100);
//                 EditorGUILayout.EndHorizontal();

//                 // Smoothing
//                 EditorGUILayout.BeginHorizontal();
//                 GUILayout.Label("Smoothing Iterations");
//                 _smoothing = EditorGUILayout.IntSlider(_smoothing, 0, 100);
//                 EditorGUILayout.EndHorizontal();
//             }
//         }

//         private void DrawDebug()
//         {
//             _debugGroup = EditorGUILayout.Foldout(_debugGroup, "Debug", _foldoutStyle);

//             if (_debugGroup)
//             {
//                 // Draw Gizmo
//                 EditorGUILayout.BeginHorizontal();
//                 GUILayout.Label("Draw Gizmo");
//                 _drawGizmo = EditorGUILayout.Toggle(_drawGizmo);
//                 EditorGUILayout.EndHorizontal();
//             }
//         }

//         void BeginDraw(Rect r)
//         {
//             if (_renderer == null)
//                 _renderer = new PreviewRenderUtility();

//             _renderer.camera.transform.position = new Vector3(0, 2.5f, 0);
//             _renderer.camera.transform.LookAt(Vector3.zero, Vector3.up);
//             _renderer.camera.farClipPlane = 30;
//             _renderer.camera.nearClipPlane = 0.1f;

//             _renderer.lights[0].intensity = 1.5f;
//             _renderer.lights[0].transform.eulerAngles = new Vector3(50, -30, 0);
//             _renderer.lights[0].color = Color.white;
//             _renderer.lights[1].intensity = 0;

//             // prefabTransform.SetTRS(prefabTranslation, Quaternion.Euler(prefabRotation), prefabScale);
//             _renderer.BeginPreview(r, GUIStyle.none); 
//         }

//         public void DrawRenderPreview()
//         {
//             if (_gameObject != null)
//             {
//                 MeshFilter[] meshFilters = _gameObject.GetComponentsInChildren<MeshFilter>();
//                 for (int i = 0; i < meshFilters.Length; i++)
//                 {
//                     if (meshFilters[i].sharedMesh)
//                     {
//                         MeshRenderer meshRenderer = meshFilters[i].gameObject.GetComponent<MeshRenderer>();
//                         for (int j = 0; j < meshFilters[i].sharedMesh.subMeshCount; j++)
//                             if (meshRenderer != null)
//                             {
//                                 _renderer.DrawMesh(
//                                     meshFilters[i].sharedMesh,
//                                     // prefabTransform * meshRenderer.transform.localToWorldMatrix,
//                                     meshRenderer.transform.localToWorldMatrix,
//                                     meshRenderer.sharedMaterials[j],
//                                     j);
//                             }
//                     }
//                 }
//             }
//         }

//         void EndDraw(Rect r)
//         {
//             bool fog = RenderSettings.fog;
//             Unsupported.SetRenderSettingsUseFogNoDirty(false);
//             _renderer.camera.Render();
//             Unsupported.SetRenderSettingsUseFogNoDirty(fog);

//             Texture texture = _renderer.EndPreview();
//             GUI.DrawTexture(r, texture);
//         }

//         private void OnDisable()
//         {
//             if (_renderer != null) _renderer.Cleanup();
//         }

//         private Texture2D MakeTexture(Color color)
//         {
//             Texture2D texture = new Texture2D(1, 1);
//             texture.SetPixel(1, 1, color);
//             texture.Apply();
//             return texture;
//         }
//     }
// }