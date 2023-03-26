using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    [System.Serializable]
    public class VariableInfo
    {
        public string Name;
        public object Value;
    }
    
    public class KouhaiVariables : MonoBehaviour
    {
        private readonly List<string> ignoreSymbs = new List<string>()
        {
            "_VERSION",
            KouhaiScriptEnvVars.ENV_PLAYING
        };

        [SerializeField, HideInInspector]
        private List<VariableInfo> variableInfos;

        public System.Action onUpdated;
        
        public List<VariableInfo> Variables => variableInfos;

        private Script target;
        
        public void ForceUpdateVariables()
        {
            if(target == null)
                return;
            
            Load(target);
        }
        
        public void Load(Script scriptTarget)
        {
            if (target == null)
                target = scriptTarget;
            
            if (variableInfos == null)
                variableInfos = new List<VariableInfo>();
            
            variableInfos.Clear();
            foreach (var item in target.Globals.Pairs)
            {
                if(!IsSupported(item))
                    continue;

                if(ignoreSymbs.Contains(item.Key.String))
                    continue;
                
                var current = variableInfos.Find(a => a.Name.Equals(item.Key));
                if (current != null)
                    continue;
                
                variableInfos.Add(new VariableInfo()
                {
                    Name = item.Key.String,
                    Value = item.Value.ToObject()
                });
            }
            onUpdated?.Invoke();
        }

        public void Clear()
        {
            if (variableInfos == null)
                return;

            target = null;
            variableInfos.Clear();
            onUpdated?.Invoke();
        }
        
        private bool IsSupported(TablePair pair)
        {
            List<DataType> supported = new List<DataType>() { DataType.Boolean, DataType.Number, DataType.String };
            return supported.Contains(pair.Value.Type);
        }
    }
}