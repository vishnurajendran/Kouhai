using System.Collections;
using System.Collections.Generic;
using Kouhai.Core;
using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Proxies
{
    [MoonSharpUserData]
    public class KouhaiUtilsProxy : KouhaiRuntimeProxy
    {
        [MoonSharpHidden]
        public override string Symbol => "Utils";
        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new KouhaiUtilsProxy();
        }

        public void WaitForSeconds(float seconds)
        {
            ownerScript.SetGlobal(KouhaiGlobals.SCRIPT_WAIT_TIME_KEY,seconds);
        }
    }
}
