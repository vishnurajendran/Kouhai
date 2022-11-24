using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter.Editor
{
    [ScriptedImporter(1, "lua")]
    public class KouhaiLuaScriptImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var localPath = ctx.assetPath;
            var ico = Resources.Load<Texture2D>("klua");
            KouhaiLuaScript script = ScriptableObject.CreateInstance<KouhaiLuaScript>();
            script.Initialise(File.ReadAllText(localPath));
            ctx.AddObjectToAsset(localPath, script, ico); // Pass your icon here
            ctx.SetMainObject(script);
        }
    }
}
