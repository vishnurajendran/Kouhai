using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Kouhai.Scripting.Serialisation
{
    public static class DynValueSerialiser
    {
        private static bool initialised;

        private static JsonSerializerSettings settings;
        private static Dictionary<DataType, IDVSerialisationContext> serialisationContexts;
        
        /// <summary>
        /// Serialise DynValue
        /// </summary>
        /// <param name="value">value to serialise</param>
        /// <returns></returns>
        public static string Serialise(DynValue value)
        {
            if (!initialised)
                Initialise();
            if (!serialisationContexts.ContainsKey(value.Type))
                return string.Empty;
            var container = new SerialisedDynValueContainer()
            {
                Type = value.Type,
                Value = serialisationContexts[value.Type].Serialise(value)
            };
            return JsonConvert.SerializeObject(container, settings);
        }

        /// <summary>
        /// Deserialise string to DynValue
        /// </summary>
        /// <param name="serialisedString">serialised data to de-serialise</param>
        /// <returns></returns>
        public static DynValue Deserialise(Script script, string serialisedString)
        { 
            if (!initialised)
                Initialise();
            var container = JsonConvert.DeserializeObject<SerialisedDynValueContainer>(serialisedString, settings);
            if (!serialisationContexts.ContainsKey(container.Type))
                return DynValue.Nil;
            return serialisationContexts[container.Type].Deserialise(script, container.Value);
        }
        
        private static void Initialise()
        {
            if (initialised)
                return;
            LoadSettings();
            LoadSerialisationContexts();
            initialised = true;
        }

        private static void LoadSettings()
        {
            settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
        }
        
        private static void LoadSerialisationContexts()
        {
            var type = typeof(IDVSerialisationContext);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            
            serialisationContexts = new Dictionary<DataType, IDVSerialisationContext>();
            foreach (var t in types)
            {
                if (t == type)
                    continue;
                
                var instance = Activator.CreateInstance(t) as IDVSerialisationContext;
                if(instance == null)
                    continue;
                if(!serialisationContexts.ContainsKey(instance.SupportedType))
                    serialisationContexts.Add(instance.SupportedType, instance);
            }
        }
    }
}
