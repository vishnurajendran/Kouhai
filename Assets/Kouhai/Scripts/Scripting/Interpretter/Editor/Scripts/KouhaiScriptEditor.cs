using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Kouhai.Scripting.Interpretter.KouhaiScript))]
[CanEditMultipleObjects]
public class KouhaiScriptEditor : Editor
{
    private static readonly string[] fieldExclusions = new string[] { "m_Script" };

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}
