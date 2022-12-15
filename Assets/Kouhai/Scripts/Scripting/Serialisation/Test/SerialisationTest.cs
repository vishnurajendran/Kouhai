using System.Collections;
using System.Collections.Generic;
using Kouhai.Scripting.Serialisation;
using MoonSharp.Interpreter;
using UnityEngine;

public class SerialisationTest : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string serialisedData;

    [SerializeField] private SerializableDictionary<string, string> data;
    void Start()
    {
        var script = new Script();
        var value = DynValue.NewTable(script);
        value.Table["number"] = 5;
        value.Table["bool"] = true;
        value.Table["string"] = "Hello World!";

        var subTable = DynValue.NewTable(script);
        subTable.Table["subNumber"] = 7;
        subTable.Table["subBool"] = false;
        
        value.Table["table"] = subTable;
        serialisedData = DynValueSerialiser.Serialise(value);
        var dynValueDesrialisedData = DynValueSerialiser.Deserialise(script, serialisedData);
        data = new SerializableDictionary<string, string>();
        foreach (var key in dynValueDesrialisedData.Table.Keys)
        {
            data.Add(key.ToString(), dynValueDesrialisedData.Table[key].ToString());
        }
    }
}
