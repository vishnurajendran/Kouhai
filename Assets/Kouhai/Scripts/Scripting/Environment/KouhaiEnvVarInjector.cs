using System;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    public static class KouhaiEnvVarInjector
    {
        public static void Inject(Interpretter.KouhaiScript script)
        {
            script.luaScript.Globals[KouhaiScriptEnvVars.ENV_PLAYING] = ApplicationPlaying();
        }
        
        private static bool ApplicationPlaying()
        {
            return Application.isPlaying;
        }
    }
}