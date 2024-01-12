using System;
using Kouhai.Constants;

namespace Kouhai.Runtime.System
{
    [Serializable]
    public class KouhaiDownloadRequest
    {
        public string Id;
        public string DownloadTitle;
        public string Url;
        public string Destination = KouhaiDownloadConstants.KouhaiDownloadsDirectory;
    }
}