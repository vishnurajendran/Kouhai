using UnityEditor;
using UnityEngine;

namespace LeTai.TrueShadow.Editor
{
public static class Utility
{
    public static T FindAsset<T>(string assetName) where T : UnityEngine.Object
    {
        var guids = AssetDatabase.FindAssets("l:TrueShadowEditorResources " + assetName);
        if (guids.Length == 0)
        {
            Debug.LogError($"Asset \"{assetName}\" not found. " +
                           $"Make sure it have the label \"TrueShadowEditorResources\"");
            return null;
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
}
