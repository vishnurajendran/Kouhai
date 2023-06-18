using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kouhai.Core.AssetManagement;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    public class KouhaiLuaScriptLoader : ScriptLoaderBase
    {
        private const string SCRIPTS_PATH = "Scripts/";
        private static readonly Dictionary<string, string> scripts = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> precompiledScripts = new Dictionary<string, string>();
        private static bool isInitialised = false;

        private KouhaiLuaScript[] GetAllScripts()
        {
#if UNITY_EDITOR && !KOUHAI_APP_TESTING
            return Resources.LoadAll<KouhaiLuaScript>(SCRIPTS_PATH);
#else
            if(Application.isPlaying)
                return KouhaiAssetManager.LoadAssetAll<KouhaiLuaScript>(SCRIPTS_PATH);
            
            return Resources.LoadAll<KouhaiLuaScript>(SCRIPTS_PATH);
#endif
        }
        
        private void Init()
        {
            KouhaiLuaScript[] result = GetAllScripts();
            if(result == null)
                return;
            
            foreach (var klua in result)
            {
                scripts.Add(klua.name, klua.Source);
            }
            Script.DefaultOptions.ScriptLoader = this;
            isInitialised = true;
        }

        private string CompileSource(string luaSourceCode)
        {
            return KouhaiPreCompiler.PreCompile(luaSourceCode);
        }
        
        public DynValue LoadScriptSource(Script script, string sourceFileName)
        {
            if (!isInitialised)
                Init();

            if (string.IsNullOrEmpty(sourceFileName))
            {
                Debugging.KouhaiDebug.LogError($"Unable to load source, requested filename is null or empty");
                return DynValue.Nil;
            }

            if (!scripts.ContainsKey(sourceFileName))
            {
                Debugging.KouhaiDebug.LogError($"Unable to load source for {sourceFileName}");
                return DynValue.Nil;
            }

            script.Options.ScriptLoader = this;
            var source = "";
            
            if(precompiledScripts.ContainsKey(sourceFileName))
                source = precompiledScripts[sourceFileName];
            else
            {
                source = KouhaiPreCompiler.PreCompile(scripts[sourceFileName]);
                precompiledScripts.Add(sourceFileName, source);
            }
            return script.LoadString(source);
        }

        public override object LoadFile(string file, Table globalContext)
        {
            return scripts[file];
        }

        public override bool ScriptFileExists(string name)
        {
            return scripts.ContainsKey(name);
        }

        public override string ResolveModuleName(string modname, Table globalContext)
        {
            return modname;
        }

        protected override string ResolveModuleName(string modname, string[] paths)
        {
            return modname;
        }

        public override string ResolveFileName(string filename, Table globalContext)
        {
            return filename;
        }
    }
}
