using Kouhai.Scripting.Interpretter;
using Kouhai.Scripting.Serialisation;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Proxies
{
    [MoonSharpUserData]
    public class GameSaverProxy : KouhaiRuntimeProxy
    {
        [MoonSharpHidden]
        public override string Symbol => "SaveSystem";
        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new GameSaverProxy();
        }
    
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    
        public void SaveData(string key, DynValue value)
        {
            var serialisedData = DynValueSerialiser.Serialise((value));
            Debug.Log($"Saving {serialisedData} with Key {key}");
            PlayerPrefs.SetString(key, serialisedData);
        }

        public DynValue GetData(Script script, string key)
        {
            Debug.Log("Script is Null? " + (script == null));
            if(!HasKey(key))
                return DynValue.Nil;

            return DynValueSerialiser.Deserialise(script,
                PlayerPrefs.GetString(key));
        }
    
    }
 
}
