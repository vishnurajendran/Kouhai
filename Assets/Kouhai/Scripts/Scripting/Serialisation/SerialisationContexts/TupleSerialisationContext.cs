using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using Newtonsoft.Json;

namespace Kouhai.Scripting.Serialisation
{
    public class TupleSerialisationContext : IDVSerialisationContext
    {
        public DataType SupportedType => DataType.Tuple;
        public DynValue Deserialise(Script script, string serialisedString)
        {
            var newSerialisedData = JsonConvert.DeserializeObject<List<string>>(serialisedString);
            List<DynValue> values = new List<DynValue>();
            foreach (var data in newSerialisedData)
            {
                values.Add(DynValueSerialiser.Deserialise(script, data));
            }
            return DynValue.NewTuple(values.ToArray());
        }

        public string Serialise(DynValue dynValue)
        {
            var list = new List<string>();
            foreach (var key in dynValue.Tuple)
            {
                list.Add(DynValueSerialiser.Serialise(dynValue.Table.Get(key)));
            }
            return JsonConvert.SerializeObject(list);
        }
    }
}