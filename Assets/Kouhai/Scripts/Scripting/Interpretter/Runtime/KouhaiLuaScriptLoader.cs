using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    public class KouhaiLuaScriptLoader : ScriptLoaderBase
    {
        private const string SCRIPTS_PATH = "Scripts/";
        private static Dictionary<string, string> scripts = new Dictionary<string, string>();
        private static bool isInitialised = false;

        public void Init()
        {
            KouhaiLuaScript[] result = Resources.LoadAll<KouhaiLuaScript>(SCRIPTS_PATH);
            foreach (var klua in result)
            {
                scripts.Add(klua.name, klua.Source);
            }
            Script.DefaultOptions.ScriptLoader = this;
            isInitialised = true;
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
            return script.LoadString(scripts[sourceFileName]);
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
