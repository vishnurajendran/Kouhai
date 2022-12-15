using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Kouhai.Scripting.Serialisation
{
    [System.Serializable]
    public class SerialisedDynValueContainer
    {
        public DataType Type;
        public string Value;
    }
}
