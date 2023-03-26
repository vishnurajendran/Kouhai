using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter.Editor
{
    [CustomEditor(typeof(KouhaiVariables))]
    public class KouhaiVariableEditor : UnityEditor.Editor
    {
        private KouhaiVariables variableTarget;
        private List<VariableDrawer> variables;

        private void OnEnable()
        {
            variableTarget = target as KouhaiVariables;
            variableTarget.onUpdated += LoadVariableData;
            LoadVariableData();
        }

        private void OnDisable()
        {
            variableTarget.onUpdated -= LoadVariableData;
        }

        private void LoadVariableData()
        {
            if (variableTarget.Variables == null)
                return;
            
            variables = new List<VariableDrawer>();
            foreach (var variable in variableTarget.Variables)
            {
                variables.Add(new VariableDrawer(variable));
            }
        }
        
        public override void OnInspectorGUI()
        {
            if (variables == null || variables.Count <= 0)
            {
                EditorGUILayout.BeginVertical("groupbox");
                EditorGUILayout.LabelField("No Variable Detected");
                EditorGUILayout.EndVertical();

                RenderRefreshButton();
                return;
            }
            
            EditorGUILayout.BeginVertical("groupbox");
            foreach (var variable in variables)
            {
                variable.Draw();
            }
            EditorGUILayout.EndVertical();

            RenderRefreshButton();
        }

        private void RenderRefreshButton()
        {
            if (Application.isPlaying)
                return;
            
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("Refresh"))
            {
                variableTarget.ForceUpdateVariables();
                LoadVariableData();
            }
            EditorGUILayout.EndVertical();
        }
    }

    public class VariableDrawer
    {
        private VariableInfo info;
        public VariableDrawer(VariableInfo info)
        {
            this.info = info;
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal("box");
            info.Value = ValueField(info.Name, info.Value);
            EditorGUILayout.EndHorizontal();
        }

        private object ValueField(string name,object inValue)
        {
            if (inValue is string)
            {
                return EditorGUILayout.TextField(name, (string)inValue);
            }
            
            if (inValue is double)
            {
                return EditorGUILayout.DoubleField(name,(double)inValue);
            }
            
            if (inValue is bool)
            {
                return EditorGUILayout.Toggle(name,(bool)inValue);
            }

            EditorGUILayout.LabelField(name);
            return inValue;
        }
        
    }
}
