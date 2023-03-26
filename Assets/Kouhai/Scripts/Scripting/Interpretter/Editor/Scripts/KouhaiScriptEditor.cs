using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Kouhai.Scripting.Interpretter.Editor
{
    [CustomEditor(typeof(Kouhai.Scripting.Interpretter.KouhaiScript))]
    [CanEditMultipleObjects]
    public class KouhaiScriptEditor : UnityEditor.Editor
    {
        private static readonly string[] fieldExclusions = new string[] { "m_Script" };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

    }
}
