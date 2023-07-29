using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace LeTai.TrueShadow.Editor
{
[CanEditMultipleObjects]
[CustomEditor(typeof(TrueShadow))]
public class TrueShadowEditor : UnityEditor.Editor
{
    EditorProperty insetProp;
    EditorProperty sizeProp;
    EditorProperty spreadProp;
    EditorProperty angleProp;
    EditorProperty distanceProp;
    EditorProperty colorProp;
    EditorProperty blendModeProp;
    EditorProperty multiplyCasterAlphaProp;
    EditorProperty ignoreCasterColorProp;
    EditorProperty colorBleedModeProp;

    GUIContent procrastinateLabel;

    static bool showExperimental;
    static bool showAdvanced;

    void OnEnable()
    {
        insetProp               = new EditorProperty(serializedObject, nameof(TrueShadow.Inset));
        sizeProp                = new EditorProperty(serializedObject, nameof(TrueShadow.Size));
        spreadProp              = new EditorProperty(serializedObject, nameof(TrueShadow.Spread));
        angleProp               = new EditorProperty(serializedObject, nameof(TrueShadow.OffsetAngle));
        distanceProp            = new EditorProperty(serializedObject, nameof(TrueShadow.OffsetDistance));
        colorProp               = new EditorProperty(serializedObject, nameof(TrueShadow.Color));
        blendModeProp           = new EditorProperty(serializedObject, nameof(TrueShadow.BlendMode));
        multiplyCasterAlphaProp = new EditorProperty(serializedObject, nameof(TrueShadow.UseCasterAlpha));
        ignoreCasterColorProp   = new EditorProperty(serializedObject, nameof(TrueShadow.IgnoreCasterColor));
        colorBleedModeProp      = new EditorProperty(serializedObject, nameof(TrueShadow.ColorBleedMode));

        if (EditorPrefs.GetBool("LeTai_TrueShadow_" + nameof(showExperimental)))
        {
            showExperimental = EditorPrefs.GetBool("LeTai_TrueShadow_" + nameof(showExperimental), false);
            showAdvanced     = EditorPrefs.GetBool("LeTai_TrueShadow_" + nameof(showAdvanced),     false);
        }

        procrastinateLabel = new GUIContent("Procrastinate", "A bug that is too fun to fix");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var ts = (TrueShadow) target;

        insetProp.Draw();
        sizeProp.Draw();
        spreadProp.Draw();
        angleProp.Draw();
        distanceProp.Draw();
        colorProp.Draw();
        if (ts.UsingRendererMaterialProvider)
        {
            using(new EditorGUI.DisabledScope(true))
                LabelField(blendModeProp.serializedProperty.displayName, "Custom Material");
        }
        else
        {
            blendModeProp.Draw();
        }

        using (var change = new EditorGUI.ChangeCheckScope())
        {
            showAdvanced = Foldout(showAdvanced, "Advanced Settings", true);
            using (new EditorGUI.IndentLevelScope())
                if (showAdvanced)
                {
                    multiplyCasterAlphaProp.Draw();
                    ignoreCasterColorProp.Draw();
                    colorBleedModeProp.Draw();

                    if (KnobPropertyDrawer.procrastinationMode)
                    {
                        var rot = GUI.matrix;
                        GUI.matrix                             =  Matrix4x4.identity;
                        KnobPropertyDrawer.procrastinationMode ^= Toggle("Be Productive", false);
                        GUI.matrix                             =  rot;
                    }
                    else
                    {
                        KnobPropertyDrawer.procrastinationMode |= Toggle(procrastinateLabel, false);
                    }
                }

            if (change.changed)
            {
                EditorPrefs.SetBool("LeTai_TrueShadow_" + nameof(showExperimental), showExperimental);
                EditorPrefs.SetBool("LeTai_TrueShadow_" + nameof(showAdvanced),     showAdvanced);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
}
