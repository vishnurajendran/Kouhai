using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kouhai.Constants;
using Newtonsoft.Json;
using UnityEngine;

namespace Kouhai.Runtime.System
{
   [Serializable]
    public class KouhaiDownloadEntry
    {
        public string Id;
        public string DownloadTitle;
        public string SourceUrl;
        public string DestinationDirectory;
        public float CurrentProgress = 0;
        public long EstimatedFileSize = 0;
        public string PathInDisk => $"{DestinationDirectory}/{Path.GetFileName(SourceUrl)}";
        public bool IsCompleted => CurrentProgress >= 1.0f;
        public int bufferSize;
        
        /// <summary>
        /// Invoked when download size is fetched
        /// </summary>
        [JsonIgnore]
        public Action<long> OnDownloadSizeFetched;
        /// <summary>
        /// Invoked on download progress,
        /// Total, Download
        /// </summary>
        [JsonIgnore]
        public Action<float, float> OnDownloadProgress;
        /// <summary>
        /// Finalisation of download
        /// </summary>
        [JsonIgnore]
        public Action OnDownloadFinalising;
        /// <summary>
        /// Cancellation of download
        /// </summary>
        [JsonIgnore]
        public Action OnDownloadCancelled;
        /// <summary>
        /// Completion of download
        /// </summary>
        [JsonIgnore]
        public Action<bool> OnDownloadCompleted;

        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        private HttpClient httpClient;

        private Action<KouhaiDownloadEntry> internallCompleteCallback;
        
        internal void SetInternalCompleteCallback(Action<KouhaiDownloadEntry> internallCompleteCallback)
        {
            this.internallCompleteCallback = internallCompleteCallback;
        }
        
        private string TempPathInDisk => $"{DestinationDirectory}/TEMP_{Id}{KouhaiDownloadConstants.KOUHAI_DOWNLOAD_EXT}";
        private string MetaPath => $"{DestinationDirectory}/{Id}_meta{KouhaiDownloadConstants.KOUHAI_DOWNLOAD_META_EXT}";

        public KouhaiDownloadEntry()
        {
        }
        
        public KouhaiDownloadEntry(KouhaiDownloadRequest request,int bufferSize)
        {
            this.Id = request.Id;
            this.DownloadTitle = request.DownloadTitle;
            this.SourceUrl = request.Url;
            this.DestinationDirectory = request.Destination;
            this.bufferSize = bufferSize;
        }
        
        public async Task StartOrContinueDownload()
        {
            if (tokenSource != null)
            {
                Debug.Log("Welps!");
                return; //we dont proceed, if we already have a download active for this entry
            }

            //validate destination dir
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }
            
            if (!IsCompleted)
            {
                tokenSource = new CancellationTokenSource();
                
#if DOWNLOAD_RESUME_SUPPORTED
                if (CurrentProgress <= 0f)
                   await StartDownload();
                else
                   await ContinueDownload();
#else
                await StartDownload();
#endif
            }
        }

        public void CancelDownload()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            token = default;
        }

#if DOWNLOAD_RESUME_SUPPORTED
        private async Task ContinueDownload()
        {
            Debug.LogError("This feature is currently not supported");
            //TODO: Add support for download resuming functionality
            //download resume is not currently supported
            //this feature requires server side supported
            //this shall be added once server side support
            //is added.
        }
#endif
        
        private async Task StartDownload()
        {
            httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
            using var response = await httpClient.GetAsync(SourceUrl, HttpCompletionOption.ResponseHeadersRead);
            await DownloadFileFromHttpResponseMessage(response);
        }

        private async Task UpdateMetaData()
        {
            await File.WriteAllTextAsync(MetaPath, JsonConvert.SerializeObject(this));
        }

        private void DeleteMetaPath()
        {
            File.Delete(MetaPath);
        }
        
        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            EstimatedFileSize = response.Content.Headers.ContentLength ?? 0;
            await UpdateMetaData();
            OnDownloadSizeFetched?.Invoke(EstimatedFileSize);
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                await ProcessContentStream(contentStream);
                DeleteMetaPath();
            }
        }

        private async Task ProcessContentStream(Stream contentStream)
        {
            var totalBytesRead = 0L;
            var buffer = new byte[bufferSize];
            var isMoreToRead = true;
            var fileStream = new FileStream(TempPathInDisk, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
            CurrentProgress = 0;
            do
            {
                var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    CurrentProgress = 1;
                    isMoreToRead = false;
                    OnDownloadProgress?.Invoke(EstimatedFileSize, totalBytesRead);
                    break;
                }

                await fileStream.WriteAsync(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;
                CurrentProgress = (float)totalBytesRead / EstimatedFileSize;
                OnDownloadProgress?.Invoke(EstimatedFileSize, totalBytesRead);

            } while (isMoreToRead && !token.IsCancellationRequested);
            
            await fileStream.DisposeAsync();
            
            if (token.IsCancellationRequested)
            {
                OnDownloadCancelled?.Invoke();
                File.Delete(TempPathInDisk);
                OnDownloadCompleted?.Invoke(false);
                return;
            }

            OnDownloadFinalising?.Invoke();
            File.Move(TempPathInDisk, PathInDisk);
            OnDownloadCompleted?.Invoke(true);
            internallCompleteCallback?.Invoke(this);
        }
        
        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}