using System.Collections.Generic;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using UnityEngine;

namespace Kouhai.Scripting.Serialisation
{
    public class TableSerialisationContext : IDVSerialisationContext
    {
        public DataType SupportedType => DataType.Table;
        public DynValue Deserialise(Script script,string serialisedString)
        {
            var newSerialisedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialisedString); 
            DynValue table = DynValue.NewTable(script);
            foreach (var tableData in newSerialisedData)
            {
                table.Table[tableData.Key] = DynValueSerialiser.Deserialise(script, tableData.Value);
            }

            return table;
        }

        public string Serialise(DynValue dynValue)
        {
            var tableDict = new Dictionary<string, string>();
            foreach (var key in dynValue.Table.Keys)
            {
                var keyName = key.ToString();
                keyName = keyName.Remove(0, 1);
                keyName = keyName.Remove(keyName.Length - 1, 1);
                tableDict.Add(keyName, DynValueSerialiser.Serialise(dynValue.Table.Get(key)));
            }
            return JsonConvert.SerializeObject(tableDict);
        }
    }
}
