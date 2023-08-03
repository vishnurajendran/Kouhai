using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Kouhai.Constants;
using Newtonsoft.Json;

namespace Kouhai.Runtime.System
{
    [Serializable]
    public class KouhaiDownloadRequest
    {
        public string DownloadTitle;
        public string Url;
        public string Destination;
    }
    
    public static class KouhaiDownloadManager
    {
        public static KouhaiDownloadEntry CreateDownloadEntry(KouhaiDownloadRequest downloadRequest, int bufferSize = KouhaiConstants.MAX_DOWNLOAD_BUFFER)
        {
            var instance = new KouhaiDownloadEntry(downloadRequest, bufferSize);
            return instance;
        }
    }

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
        /// Completion of download
        /// </summary>
        [JsonIgnore]
        public Action OnDownloadCompleted;
        
        private HttpClient httpClient;
        private int bufferSize;
        
        private string TempPathInDisk => $"{DestinationDirectory}/TEMP_{Id}.khdownload";
        private string MetaPath => $"{DestinationDirectory}/{Id}_meta.kdownloadmeta";
        
        public KouhaiDownloadEntry(KouhaiDownloadRequest request, int bufferSize = 8192)
        {
            this.Id = Guid.NewGuid().ToString().Replace("-","");
            this.DownloadTitle = request.DownloadTitle;
            this.SourceUrl = request.Url;
            this.DestinationDirectory = request.Destination;
            this.bufferSize = bufferSize;
        }
        
        public async Task StartOrContinueDownload()
        {
            //validate destination dir
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }
            
            if (!IsCompleted)
            {
#if DOWNLOAD_RESUME_SUPPORTED
                await ContinueDownload();
#else
                await StartDownload();
#endif
            }
            else
            {
                await StartDownload();
            }
        }

#if DOWNLOAD_RESUME_SUPPORTED
        private async Task ContinueDownload()
        {
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
            await using (var fileStream = new FileStream(TempPathInDisk, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
            {
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
                    await UpdateMetaData();
                    totalBytesRead += bytesRead;
                    CurrentProgress = (float)totalBytesRead / EstimatedFileSize;
                    OnDownloadProgress?.Invoke(EstimatedFileSize, totalBytesRead);
                }
                while (isMoreToRead);
            }
            OnDownloadFinalising?.Invoke();
            File.Move(TempPathInDisk, PathInDisk);
            OnDownloadCompleted?.Invoke();
        }
        
        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}

