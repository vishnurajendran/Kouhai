using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Scripting.Environment
{
    public class KouhaiEnv
    {
        private static bool initialied = false;
        private static KouhaiLuaScriptLoader scriptLoader;

        private static void Initialise()
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            scriptLoader = new KouhaiLuaScriptLoader(); 
            initialied = true;
        }

        public static void SetupScript(Interpretter.KouhaiScript script, string fileName)
        {
            if (!initialied)
                Initialise();

            scriptLoader.LoadScriptSource(script.luaScript, fileName);
            PrepareScript(script);
        }

        private static void PrepareScript(Interpretter.KouhaiScript script)
        {
            ReflectionProxyRegisterar.RegisterFor(script);
        }

    }
}
