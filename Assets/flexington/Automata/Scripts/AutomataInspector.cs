using UnityEngine;
using UnityEditor;

namespace flexington.Automata
{
    [CustomEditor(typeof(AutomataComponent))]
    public class AutomataInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate and Simulate"))
            {
                ((AutomataComponent)target).GenerateAndSimulate();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate"))
            {
                ((AutomataComponent)target).Generate();
            }

            if (GUILayout.Button("Next Generation"))
            {
                ((AutomataComponent)target).NextGeneration();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset"))
            {
                ((AutomataComponent)target).Reset();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}

