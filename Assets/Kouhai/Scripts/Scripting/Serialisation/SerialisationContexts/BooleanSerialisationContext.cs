using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Serialisation
{
    public class BooleanSerialisationContext : IDVSerialisationContext
    {
        public DataType SupportedType => DataType.Boolean;
        
        public DynValue Deserialise(Script script,string serialisedString)
        {
            var value = System.Convert.ToInt32(serialisedString);
            bool boolean = value >= 1 ? true : false;
            return DynValue.NewBoolean(boolean);
        }
        
        public string Serialise(DynValue dynValue)
        {
            return dynValue.Boolean?"1":"0";
        }
    }
}
