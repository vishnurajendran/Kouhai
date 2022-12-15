using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Serialisation
{
    public class NumberSerialisationContext : IDVSerialisationContext
    {
        public DataType SupportedType => DataType.Number;
        
        public DynValue Deserialise(Script script,string serialisedString)
        {
            return DynValue.NewNumber(System.Convert.ToDouble(serialisedString));
        }

        public string Serialise(DynValue dynValue)
        {
            return dynValue.ToString();
        }
    }
}
