using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter {

    public abstract class KouhaiRuntimeProxy
    {
        public KouhaiScript ownerScript;

        public const string SYMBOL_NAME_PROP = "Symbol";
        public const string PROXY_INSTANCE_MTHD = "GetProxyInstance";

        public abstract string Symbol { get; }
        public abstract KouhaiRuntimeProxy GetProxyInstance();
    }
}
