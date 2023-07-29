using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Kouhai.Scripts.Runtime.Utils;
using UnityEngine;

namespace Kouhai.Runtime.System
{
    public class KouhaiDownloadRequest
    {
        public string DownloadTitle;
        public string Url;
        public string Destination;
    }

    public class KouhaiDownloadEntry
    {
        public string DownloadTitle;
        public string Source;
        public float CurrentProgress = 0;
        public long EstimatedFileSize = 0;
        public string PathInDisk => $"{Application.persistentDataPath}/Downloads/{DownloadTitle}/{Path.GetFileName(Source)}";
        public bool IsCompleted => CurrentProgress >= 1.0f;
    }
    
    public static class KouhaiDownloadManager
    {
        
        public static void StartDownload(KouhaiDownloadRequest downloadRequest)
        {
        
        }

        private static async void DownloadFile(KouhaiDownloadRequest downloadRequest)
        {
        
        }
    
    }
}

