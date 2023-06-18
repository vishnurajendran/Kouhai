using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kouhai.Scripting.Interpretter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public class ScriptAssetList : IAssetList
    {
        private Dictionary<string, KouhaiLuaScript> scriptAssetMap;

        public Type TargetType => typeof(KouhaiLuaScript);

        public async Task Initialise(string assetDir, Dictionary<string, string> assetMap)
        {
            var imgPath = $"{assetDir}/Scripts";
            scriptAssetMap = new Dictionary<string, KouhaiLuaScript>();
            foreach (var file in Directory.EnumerateFiles(imgPath, "*.*", SearchOption.AllDirectories))
            {
                var search = GetKeyForFileInAssetMap(assetMap, file);
                if (!string.IsNullOrEmpty(search.Value))
                {
                    var script = await LoadScript(file);
                    var str = search.Key.Replace(Path.GetExtension(search.Key),"");
                    scriptAssetMap.Add(str, script);
                }
            }
            Debug.Log($"Loaded {scriptAssetMap.Count()} scripts");
        }

        private KeyValuePair<string, string> GetKeyForFileInAssetMap(Dictionary<string, string> dict, string value)
        {
            var res = dict.FirstOrDefault(a => PathFormat(a.Value).Equals(PathFormat(value)));
            return res;
        }
 
        private string PathFormat(string input)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            return input.Replace("/", "\\");
#else
            return input.Replace("\\", "/");
#endif
        }
        
        private async Task<KouhaiLuaScript> LoadScript(string path)
        {
            var fileData = await File.ReadAllTextAsync(path);
            var klscp = ScriptableObject.CreateInstance<KouhaiLuaScript>();
            klscp.name = Path.GetFileNameWithoutExtension(path);
            klscp.Initialise(fileData);
            return klscp;
        }
        
        public Object GetAsset(string path)
        {
            if (scriptAssetMap.ContainsKey(path))
                return scriptAssetMap[path];
            
            return null;
        }
        
        public Object[] GetAssetAll(string path)
        {
            var res = new List<Object>();
            foreach (var kv in scriptAssetMap)
            {
                if (kv.Key.Contains(path))
                {
                    res.Add(kv.Value);
                }
            }
            return res.ToArray();
        }

        public async Task Cleanup()
        {
            var totalSize = 0;
            var count = 0;
            foreach (var kv in scriptAssetMap)
            {
                totalSize += kv.Value.Source.Length;
                count++;
                if(Application.isPlaying)
                    Object.Destroy(kv.Value);
                else
                    Object.DestroyImmediate(kv.Value);
                
                await Task.Yield();
            }
            scriptAssetMap.Clear();
            Debug.Log($"<color=cyan>unloaded {count} Textures ({(float)totalSize / (8 * 1024 * 1024):F1} MB)</color>");
        }
    }
}