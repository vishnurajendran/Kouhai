using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using System;

namespace Kouhai.Scripting.Proxies
{
    [MoonSharpUserData]
    public class DebugProxy : KouhaiRuntimeProxy
    {
        [MoonSharpHidden]
        public override string Symbol => "Debug";

        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new DebugProxy();
        }

        public void Log(string msg)
        {
            Debugging.KouhaiDebug.LogColor(msg, UnityEngine.Color.cyan);
        }

        public void LogError(string error)
        {
            Debugging.KouhaiDebug.LogError(error);
        }

        public void LogWarning(string warning)
        {
            Debugging.KouhaiDebug.LogWarning(warning);
        }
    }
}
