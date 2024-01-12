using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Constants
{
    public static class KouhaiStoryPackConstants
    {
        public const string PACK_EXTENSION = "khpak";
        public const string PUBLISH_FILENAME = ".stpub";
        public const string ASSET_MAP = ".assetmap";
        public const string ASSET_PAK_FILENAME = ".asset";
        public const string UNPACKED_ASSET_DIR = "Assets";
        public const string RELDIR_TMP = "[RelativeDirectory]";
    }
    
    public static class KouhaiSceneConstants
    {
        public const string PLAYER_SCENE = "KouhaiPlayer";
        public const string HOME_SCENE = "KouhaiHome";
    }
    
    public static class KouhaiDownloadConstants
    {
        public const int DOWNLOAD_BUFFER = 1024 * 1024;
        public static string KouhaiDownloadsDirectory => $"{Application.persistentDataPath}/Downloads";
        public const string KOUHAI_DOWNLOAD_EXT = ".khdownload";
        public const string KOUHAI_DOWNLOAD_META_EXT = ".kdownloadmeta";
    }
  
}
