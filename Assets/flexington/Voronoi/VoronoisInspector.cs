using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace flexington.Voronoi
{
    [CustomEditor(typeof(VoronoiComponent))]
    public class VoronoisInspector : Editor
    {
        public VoronoiComponent Target
        {
            get { return (VoronoiComponent)target; }
        }

        private Event _event;

        private void Awake()
        {
            if (_event == null) _event = Event.current;
        }

        private void OnSceneGUI()
        {
            // if (_event == null) _event = Event.current;
            // Vector3 mousePosition = _event.mousePosition;
            // Vector2 position = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

            // if (_event.type == EventType.MouseDown && _event.button == 0)
            // {
            //     Target.AddNewRegion(position);
            // }

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate and Simulate"))
            {
                Target.Start();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate"))
            {
                // Target.Seed = DateTime.Now.ToString();
                // Target.InitRegions();
                // Target.GenerateTexture();
                // Target.ApplyTexture();
            }
            if (GUILayout.Button("Grow"))
            {
                // Target.Grow();
                // Target.GenerateTexture();
                // Target.ApplyTexture();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Reset"))
            {
                Target.Reset();
            }

        }
    }
}