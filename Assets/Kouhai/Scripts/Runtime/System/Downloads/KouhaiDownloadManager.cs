using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kouhai.Constants;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kouhai.Runtime.System
{
    public static class KouhaiDownloadManager
    {
        private static Dictionary<string,KouhaiDownloadEntry> downloads;
        public static List<KouhaiDownloadEntry> Downloads =>
            downloads?.Values.ToList() ?? new List<KouhaiDownloadEntry>();

        [RuntimeInitializeOnLoadMethod]
        private static async void Init()
        {
            downloads = new Dictionary<string, KouhaiDownloadEntry>();

            var downloadEntries = await GetIncompleteDownloadsInDisk();
            foreach (var downloadEntry in downloadEntries)
            {
                var entry = downloadEntry;
                entry.SetInternalCompleteCallback(RemoveFromList);
                downloads.Add(entry.Id, entry);
            }
        }
        
        private static void RemoveFromList(KouhaiDownloadEntry entry)
        {
            if(downloads == null)
                return;

            if (downloads.ContainsKey(entry.Id))
            {
                downloads.Remove(entry.Id);
            }
        }
        
        public static KouhaiDownloadEntry CreateDownloadEntry(KouhaiDownloadRequest downloadRequest, int bufferSize)
        {
            if (downloads == null)
                downloads = new Dictionary<string, KouhaiDownloadEntry>();

            if (downloads.ContainsKey(downloadRequest.Id))
                return downloads[downloadRequest.Id];
            
            var instance = new KouhaiDownloadEntry(downloadRequest, bufferSize);
            instance.SetInternalCompleteCallback(RemoveFromList);
            downloads.Add(downloadRequest.Id,instance);
            return instance;
        }

        public static async Task<List<KouhaiDownloadEntry>> GetIncompleteDownloadsInDisk(string downloadDirectory="")
        {
            if (string.IsNullOrEmpty(downloadDirectory))
            {
                downloadDirectory = KouhaiDownloadConstants.KouhaiDownloadsDirectory;
            }

            var list = new List<KouhaiDownloadEntry>();
            var allDownloadsMeta = Directory.GetFiles(downloadDirectory,
                $"*{KouhaiDownloadConstants.KOUHAI_DOWNLOAD_META_EXT}", SearchOption.AllDirectories);
            foreach (var downloadMeta in allDownloadsMeta)
            {
                var json = await File.ReadAllTextAsync(downloadMeta);
                list.Add(JsonConvert.DeserializeObject<KouhaiDownloadEntry>(json));
            }
            return list;
        }
        
        #region Tests

#if UNITY_EDITOR

        [MenuItem("Kouhai/Tests/Downloads/Start Download Test")]
        public static void TestDownloads()
        {
            TestDownload("Test 1","https://file-examples.com/storage/fe072e668b64cd6ce9c9963/2017/04/file_example_MP4_1920_18MG.mp4");
            TestDownload("Test 2","https://file-examples.com/storage/fe072e668b64cd6ce9c9963/2017/04/file_example_MP4_480_1_5MG.mp4");
            TestDownload("Test 3", "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
        }

        [MenuItem("Kouhai/Tests/Downloads/Get Incomplete Downloads")]
        public static async void TestDownloadsRetrieval()
        {
            var lst = await GetIncompleteDownloadsInDisk();
            Debug.Log($"Incomplete Downloads: {lst.Count}");
            foreach (var itm in lst)
            {
                Debug.Log($"<color=cyan>{itm.Id}, {itm.EstimatedFileSize}, {itm.DownloadTitle}, {itm.SourceUrl}, {itm.PathInDisk}</color>");
            }
        }
        
        private static void TestDownload(string title, string url)
        {
            var entry = CreateDownloadEntry(new KouhaiDownloadRequest()
            {
                Id = Guid.NewGuid().ToString().Replace("-",""),
                DownloadTitle = title,
                Url = url
            }, KouhaiDownloadConstants.DOWNLOAD_BUFFER);
            var id = Progress.Start(title, "Initialised");
            entry.OnDownloadSizeFetched += (size) => Progress.Report(id, 0f);
            entry.OnDownloadProgress += (total, current) => Progress.Report(id, (float)current/total);
            entry.OnDownloadCompleted += (success) => Progress.Finish(id);
            entry.StartOrContinueDownload();
        }

        [MenuItem("Kouhai/Tests/Downloads/Open Downloads Directory")]
        public static void OpenPersistantDirectory()
        {
            EditorUtility.RevealInFinder(KouhaiDownloadConstants.KouhaiDownloadsDirectory);
        }
        
#endif
        
#endregion
    }
}

