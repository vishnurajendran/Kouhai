using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public class SpriteAssetList : IAssetList
    {
        private Dictionary<string, Sprite> spriteAssetMap;

        public Type TargetType => typeof(Sprite);

        public async Task Initialise(string assetDir, Dictionary<string, string> assetMap)
        {
            var imgPath = $"{assetDir}/Images";
            spriteAssetMap = new Dictionary<string, Sprite>();
            foreach (var file in Directory.EnumerateFiles(imgPath, "*.*", SearchOption.AllDirectories))
            {
                var search = GetKeyForFileInAssetMap(assetMap, file);
                if (!string.IsNullOrEmpty(search.Value))
                {
                    var sprite = await LoadSprite(file);
                    var str = search.Key.Replace(Path.GetExtension(search.Key),"");
                    spriteAssetMap.Add(str, sprite);
                }
            }
            Debug.Log($"Loaded {spriteAssetMap.Count()} imgs");
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
        
        private async Task<Sprite> LoadSprite(string path)
        {
            var req = UnityWebRequestTexture.GetTexture(path);
            var asyncOp = req.SendWebRequest();
            while (!asyncOp.isDone)  await Task.Yield();
            if (req.result != UnityWebRequest.Result.Success) return null;
            var texture = DownloadHandlerTexture.GetContent(req);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
        }
        
        public Object GetAsset(string path)
        {
            if (spriteAssetMap.ContainsKey(path))
                return spriteAssetMap[path];
            
            return null;
        }
        
        public Object[] GetAssetAll(string path)
        {
            var res = new List<Object>();
            foreach (var kv in spriteAssetMap)
            {
                if (kv.Key.Contains(path))
                    res.Add(kv.Value); ;
            }
            return res.ToArray();
        }

        public async Task Cleanup()
        {
            var totalSize = 0;
            var count = 0;
            foreach (var kv in spriteAssetMap)
            {
                totalSize += kv.Value.texture.GetRawTextureData().Length;
                count++;
                if(Application.isPlaying)
                    Object.Destroy(kv.Value);
                else
                    Object.DestroyImmediate(kv.Value);
                
                await Task.Yield();
            }
            spriteAssetMap.Clear();
            Debug.Log($"<color=cyan>unloaded {count} Textures ({(float)totalSize / (8 * 1024 * 1024):F1} MB)</color>");
        }
    }
}