using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public class AudioAssetList : IAssetList
    {
        private Dictionary<string, AudioClip> audioAssetMap;
        public Type TargetType => typeof(AudioClip);

        public async Task Initialise(string assetDir, Dictionary<string, string> assetMap)
        {
            var imgPath = $"{assetDir}/Audio";
            audioAssetMap = new Dictionary<string, AudioClip>();
            foreach (var file in Directory.EnumerateFiles(imgPath, "*.*", SearchOption.AllDirectories))
            {
                var search = GetKeyForFileInAssetMap(assetMap, file);
                if (!string.IsNullOrEmpty(search.Value))
                {
                    var clip = await LoadClip(file);
                    var str = search.Key.Replace(Path.GetExtension(search.Key),"");
                    audioAssetMap.Add(str, clip);
                }
            }
            Debug.Log($"Loaded {audioAssetMap.Count()} clips");
        }

        private async Task<AudioClip> LoadClip(string path)
        {
            var req = UnityWebRequestMultimedia.GetAudioClip(path,GetAudioType(path));
            var asyncOp = req.SendWebRequest();
            while (!asyncOp.isDone)  await Task.Yield();
            if (req.result != UnityWebRequest.Result.Success) return null;
            return DownloadHandlerAudioClip.GetContent(req);
        }

        private AudioType GetAudioType(string path)
        {
            var ext = Path.GetExtension(path);
            switch (ext.Replace(".",""))
            {
                case "mp3": return AudioType.MPEG;
                case "ogg": return AudioType.OGGVORBIS;
                case "wav": return AudioType.WAV;
                case "m4a": return AudioType.ACC;
                default: return AudioType.UNKNOWN;
            }
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
        
        public Object GetAsset(string path)
        {
            if (audioAssetMap.ContainsKey(path))
                return audioAssetMap[path];
            
            return null;
        }
        
        public Object[] GetAssetAll(string path)
        {
            var res = new List<Object>();
            foreach (var kv in audioAssetMap)
            {
                if (kv.Key.Contains(path))
                    res.Add(kv.Value); ;
            }
            return res.ToArray();
        }

        public async Task Cleanup()
        {
            var count = 0;
            foreach (var kv in audioAssetMap)
            {
                count++;
                if(Application.isPlaying)
                    Object.Destroy(kv.Value);
                else
                    Object.DestroyImmediate(kv.Value);
            }
            audioAssetMap.Clear();
            Debug.Log($"<color=cyan>unloaded {count} audio clip(s)</color>");
        }
    }
}