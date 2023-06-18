using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Kouhai.Core.AssetManagement;
using Kouhai.Publishing;
using Kouhai.Scripting.Interpretter;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kouhai.Scripts.Editor.Publishing.BuildPipeline
{
    public class KouhaiBuildPipeline
    {
        public struct PipelineParameters
        {
            public KouhaiPublishingData PublishingData;
            public string TargetPath;
        }
        
        private const string ImagesRes = "Images";
        private const string AudioRes = "Audio";
        private const string ScriptsRes = "Scripts";
        
        private CancellationToken cancellationToken;
        
        public bool IsBuilding { get; private set; }
        
        public bool Build(PipelineParameters parameters)
        {
            var cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
            
            IsBuilding = true;
            var temp = CreateTempDirectory();

            try
            {
                var assetMap = new Dictionary<string, string>();
                if (parameters.PublishingData.PackImages)
                {
                    var res1 = CompileImages(temp,assetMap, cts.Cancel);
                    if (!res1 || cancellationToken.IsCancellationRequested)
                    {
                        IsBuilding = false;
                        Directory.Delete(temp, true);
                        return false;
                    }
                }

                if (parameters.PublishingData.PackAudio)
                {
                    var res2 = CompileAudio(temp,assetMap, cts.Cancel);
                    if (!res2 || cancellationToken.IsCancellationRequested)
                    {
                        IsBuilding = false;
                        Directory.Delete(temp, true);
                        return false;
                    }
                }

                var res3 = CompileScripts(temp,assetMap, cts.Cancel);
                if (!res3 || cancellationToken.IsCancellationRequested)
                {
                    IsBuilding = false;
                    Directory.Delete(temp, true);
                    return false;
                }
                
                StoryPackData.Pack(parameters.PublishingData,assetMap, temp, parameters.TargetPath);
                EditorUtility.DisplayProgressBar("Kouhai Build Pipeline", "Cleaning up...", 1);
                
                if(Directory.Exists(temp))
                    Directory.Delete(temp, true);
                
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("Build pipeline", "Story pack build successfull", "ok");
                IsBuilding = false;
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogException(ex);
                IsBuilding = false;
                return false;
            }
            return true;
        }
        
        private string CreateTempDirectory()
        {
            var tempPath = $"{Application.persistentDataPath}/Kouhai_Build_{Guid.NewGuid()}";
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }
        
        private bool CompileImages(string targetDirectory,Dictionary<string, string> assetMap, Action onCancel)
        {
            var detected = CompileFiles<Texture>(ImagesRes, assetMap, onCancel);
            if (detected == null)
                return false;
            
            CopyFiles(detected, targetDirectory);
            return true;
        }

        private bool CompileAudio(string targetDirectory,Dictionary<string, string> assetMap, Action onCancel)
        {
            var compiledList = CompileFiles<AudioClip>(AudioRes,assetMap, onCancel);
            if (compiledList == null)
                return false;
            
            CopyFiles(compiledList, targetDirectory);
            return true;
        }

        private bool CompileScripts(string targetDirectory,Dictionary<string, string> assetMap, Action onCancel)
        {
            var detected = CompileFiles<KouhaiLuaScript>(ScriptsRes, assetMap, onCancel);
            if (detected == null)
                return false;
            
            CopyFiles(detected, targetDirectory);
            return true;
        }
        
        private void CopyFiles(List<(string filePath ,string relativePath)> assetList, string assetDirectory)
        {
            foreach (var asset in assetList)
            {
                CopyFile(asset.filePath, $"{asset.relativePath.Replace(StoryPackData.RELDIR_TMP, assetDirectory)}");
            }
        }

        private void CopyFile(string sourcePath, string destinationPath)
        {
            var dirPath = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            
            EditorUtility.DisplayProgressBar("Copying...", sourcePath, 1);
            var fileAttrib = new FileInfo(sourcePath);
            File.Copy(sourcePath, $"{destinationPath}");
            EditorUtility.ClearProgressBar();
            
        }
        
        private List<(string actualFilePath, string relPath)> CompileFiles<T>(string path,Dictionary<string, string> assetMap, Action cancelEvent) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return new List<(string,string)>();
            }

            var allFiles = Resources.LoadAll<T>(path);
            if (allFiles == null || allFiles.Length <= 0)
                return new List<(string,string)>();

            var max = allFiles.Length;
            var curr = 0;
            var assetList = new List<(string,string)>();
            try
            {
                foreach (var file in allFiles)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    
                    var assetPath = AssetDatabase.GetAssetPath(file);
                    var diskfilePath = GetFullPath(assetPath);
                    if (File.Exists(diskfilePath))
                    {
                        var prog = (int)(((float)curr++/max)*100);
                        if (EditorUtility.DisplayCancelableProgressBar("Kouhai Build Pipeline", $"Adding ... {diskfilePath}", prog))
                        {
                            cancelEvent?.Invoke();
                            break;
                        }
                       
                        var split = assetPath.Split("/Resources/");
                        var filePath = split[split.Length - 1];
                        assetMap.Add(filePath,$"{StoryPackData.RELDIR_TMP}/{filePath}");
                        assetList.Add((diskfilePath,$"{StoryPackData.RELDIR_TMP}/{filePath}"));
                    }
                }
                EditorUtility.ClearProgressBar();
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Aborting Build pipeline!, Encountered an error while building. look below for more info");
                Debug.LogException(e);
                return null;
            }

            return assetList;
        }

       
        
        private string GetFullPath(string relativePath)
        {
            return Application.dataPath.Replace("/Assets", "/") + relativePath;
        }
    }
}