using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter.Editor
{
    [CustomEditor(typeof(KouhaiLuaScript))]
    public class KouhaiLuaScriptEditor : UnityEditor.Editor
    {
        private const string helpMsg = "You can edit Kouhai Scripts quickly from the editor. Any changes made are automatically saved";

        SerializedProperty text;
        void OnEnable()
        {
            text = serializedObject.FindProperty("_source");
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = true;
            var style = EditorStyles.textArea;
            style.richText = true;
            style.font = Resources.Load<Font>("SourceCodePro-Regular");
            EditorGUILayout.HelpBox(helpMsg, MessageType.Info, true);
            if (GUILayout.Button("Open in Text editor"))
            {
                AssetDatabase.OpenAsset(this.target);
            }

            EditorGUILayout.Space(20);
            text.stringValue = EditorGUILayout.TextArea(text.stringValue, style, GUILayout.Width(EditorGUIUtility.currentViewWidth - 42), GUILayout.ExpandHeight(true));

            if (serializedObject.hasModifiedProperties)
            {
                //serializedObject.ApplyModifiedProperties();
            }
        }

        Rect DrawToolbar()
        {
            var rect = EditorGUILayout.BeginHorizontal();
            rect.width = EditorGUIUtility.currentViewWidth - 37;
            EditorGUI.DrawRect(rect, Color.white * 0.5f);
            Button(new GUIContent("Open in external text editor"), OpenExternal, GUILayout.Width(250));

            EditorGUILayout.LabelField(this.target.name, EditorStyles.miniLabel, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            return rect;
        }

        void OpenExternal()
        {
            AssetDatabase.OpenAsset(this.target);
        }

        void DefocusAndRepaint()
        {
            GUI.FocusControl(null);
            Repaint();
        }
        // Function used to draw buttons in one line, Copy it and use it  elsewhere if you want ;)
        void Button(GUIContent content, Action action, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, EditorStyles.miniButtonLeft, options))
            {
                action();
            }
        }
    }
}
