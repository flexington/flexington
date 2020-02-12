using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace flexington.Voronoi2D
{
    [CustomEditor(typeof(VoronoiComponent))]
    public class VoronoiInspector : Editor
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

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate and Simulate"))
            {
                Target.Start();
            }
            if (GUILayout.Button("Reset"))
            {
                Target.Reset();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}