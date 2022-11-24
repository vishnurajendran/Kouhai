using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace Kouhai.Scripting.Environment
{
    public class ReflectionProxyRegisterar
    {
        private static Dictionary<Type, object> allproxies;

        public static void RegisterFor(Interpretter.KouhaiScript script)
        {
            if (allproxies == null) {
                allproxies = CreateInstances(GetAllProxiesInAssembly());
            }

            foreach (var proxyType in allproxies)
            {
                var symb = proxyType.Key.GetProperty(Interpretter.KouhaiRuntimeProxy.SYMBOL_NAME_PROP).GetValue(proxyType.Value);
                var instance = proxyType.Key.GetMethod(Interpretter.KouhaiRuntimeProxy.PROXY_INSTANCE_MTHD).Invoke(proxyType.Value, null);

                //set its owner
                ((Interpretter.KouhaiRuntimeProxy)instance).ownerScript = script;
                //set it to script global
                script.SetGlobal((string)symb, (Interpretter.KouhaiRuntimeProxy)instance);
            }
        }

        private static List<Type> GetAllProxiesInAssembly()
        {
            var lst = new List<Type>();
            var type = typeof(Interpretter.KouhaiRuntimeProxy);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                lst.AddRange(assembly.GetTypes().Where(p => type.IsAssignableFrom(p) && (p.GetConstructor(Type.EmptyTypes) != null)).ToList());
            }

            return lst;
        }

        private static Dictionary<Type, object> CreateInstances(List<Type> types)
        {
            Dictionary<Type, object> instances = new Dictionary<Type, object>();
            foreach(var type in types)
            {
                try
                {
                    instances.Add(type, Activator.CreateInstance(type));
                }
                catch (Exception ex)
                {
                    Debugging.KouhaiDebug.LogException(ex);
                }
            }
            return instances;
        }
    }
}
