using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Kouhai.Constants;
using Kouhai.Publishing;
using Newtonsoft.Json;
using CompressionLevel = System.IO.Compression.CompressionLevel;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    [Serializable]
    public class StoryPackData
    {
        public Dictionary<Type, IAssetList> assetListMap;
        private DirectoryInfo tempGameAssetDirectory;
        
        public static async Task<StoryPackData> LoadPack(string path, string targetDir)
        {
            var pack = new StoryPackData();
            await pack.Load(path, targetDir);
            return pack;
        }

        public static bool Pack(KouhaiPublishingData publishingData,Dictionary<string,string> assetMap, string assetDir, string targetDir)
        {
            var packageDir = $"{targetDir}/KH_TEMP_{Guid.NewGuid()}";
            Directory.CreateDirectory(packageDir);
            
            //write publish data
            var pubJson = JsonConvert.SerializeObject(publishingData);
            var pubInfoPath = $"{packageDir}/{KouhaiStoryPackConstants.PUBLISH_FILENAME}";
            File.WriteAllText(pubInfoPath, pubJson);
            
            //write asset map data
            var assetMapJson = JsonConvert.SerializeObject(assetMap);
            var assetMapPath = $"{packageDir}/{KouhaiStoryPackConstants.ASSET_MAP}";
            File.WriteAllText(assetMapPath, assetMapJson);
            
            //create archive of asset directory
            var dirInfo = new DirectoryInfo(assetDir);
            var ogName = dirInfo.Name;
            RenameDirectory(dirInfo,$"{KouhaiStoryPackConstants.UNPACKED_ASSET_DIR}");
            var assetFileName = $"{packageDir}/{KouhaiStoryPackConstants.ASSET_PAK_FILENAME}";
            ZipFile.CreateFromDirectory(dirInfo.FullName, assetFileName,CompressionLevel.Optimal,true);
            RenameDirectory(dirInfo,ogName);

            //create archive of package directory
            var version = $"{publishingData.Version.Replace(".", "_")}_{(publishingData.DevelopementMode ? "dev" : "")}";
            var targetPath = $"{targetDir}/com.{publishingData.Developer}.{publishingData.ProjectName}.{version}.{KouhaiStoryPackConstants.PACK_EXTENSION}";
            if(File.Exists(targetPath))
                File.Delete(targetPath);
            
            PackageFiles(targetPath, new List<string>() { assetFileName, pubInfoPath,assetMapPath  });
            Directory.Delete(packageDir, true);
            return true;
        }

        public static void RenameDirectory(DirectoryInfo di, string name, bool enableReplace=true)
        {
            if (di == null)
            {
                throw new ArgumentNullException("di", "Directory info to rename cannot be null");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("New name cannot be null or blank", "name");
            }
            var path = Path.Combine(di.Parent.FullName, name);
            if(enableReplace && Directory.Exists(path))
                Directory.Delete(path, true);
            di.MoveTo(path);
            return; //done
        }
        
        public static void PackageFiles(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
        }

        private async Task Load(string assetPath, string targetDirectory)
        {
            tempGameAssetDirectory = new DirectoryInfo(targetDirectory);
            await Unpack(assetPath, tempGameAssetDirectory.FullName);
        }

        private async Task Unpack(string gameFolder, string targetDir)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var assetFilePath = $"{gameFolder}/{KouhaiStoryPackConstants.ASSET_PAK_FILENAME}";
            var assetMapTargetFilePath = $"{gameFolder}/{KouhaiStoryPackConstants.ASSET_MAP}";
            
            //unpack stuff
            ZipFile.ExtractToDirectory(assetFilePath,targetDir);
            
            var assetMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(assetMapTargetFilePath));
           
            var keys = assetMap.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                assetMap[keys[i]] = assetMap[keys[i]].Replace(KouhaiStoryPackConstants.RELDIR_TMP, $"{targetDir}/{KouhaiStoryPackConstants.UNPACKED_ASSET_DIR}");
            }
            
            var isolatedAssetPath = $"{targetDir}/{KouhaiStoryPackConstants.UNPACKED_ASSET_DIR}";
            
            //Load scripts
            var scriptAssetList = AssetListGenerator.GetScriptAssetList();
            await scriptAssetList.Initialise(isolatedAssetPath,assetMap);
            
            //Load imgs
            var spriteAssetList = AssetListGenerator.GetSpriteAssetList();
            await spriteAssetList.Initialise(isolatedAssetPath, assetMap);
            
            //Load audio
            var audioAssetList = AssetListGenerator.GetAudioAssetList();
            await audioAssetList.Initialise(isolatedAssetPath,assetMap);
            
            assetListMap = new Dictionary<Type, IAssetList>();
            assetListMap.Add(audioAssetList.TargetType, audioAssetList);
            assetListMap.Add(spriteAssetList.TargetType, spriteAssetList);
            assetListMap.Add(scriptAssetList.TargetType, scriptAssetList);
           
            stopWatch.Stop();
            Debug.Log($"<color=cyan>Story Pack {Path.GetFileName(gameFolder)} loaded in {stopWatch.Elapsed.TotalSeconds}s</color>");
        }
        
        public async Task Unload()
        {
            foreach (var kv in assetListMap)
            {
                await kv.Value.Cleanup();
            }
        }
        
        public T LoadAsset<T>(string path) where T : Object
        {
            if (assetListMap == null) return null;
            if (!assetListMap.ContainsKey(typeof(T))) return null;

            var asset = assetListMap[typeof(T)].GetAsset(path);
            if (asset == null) return null;
            return asset as T;
        }
        
        public T[] LoadAssetAll<T>(string path) where T : Object
        {
            if (assetListMap == null) return null;
            if (!assetListMap.ContainsKey(typeof(T))) return null;
            var assets = assetListMap[typeof(T)].GetAssetAll(path);
            if (assets == null) return null;
            var lst = new List<T>();
            foreach (var asset in assets)
            {
                lst.Add(asset as T);
            }
            return lst.ToArray();
        }
    }
}

