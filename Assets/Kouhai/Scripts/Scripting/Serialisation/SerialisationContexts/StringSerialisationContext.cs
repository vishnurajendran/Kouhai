using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Serialisation
{
    public class StringSerialisationContext : IDVSerialisationContext
    {
        public DataType SupportedType => DataType.String;
        public DynValue Deserialise(Script script,  string serialisedString)
        {
            return DynValue.NewString(serialisedString);
        }

        public string Serialise(DynValue dynValue)
        {
            return dynValue.String;
        }
    }
}