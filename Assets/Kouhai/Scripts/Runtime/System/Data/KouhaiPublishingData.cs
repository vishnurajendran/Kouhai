using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Kouhai.Publishing
{
    [Serializable]
    public class KouhaiPublishingData
    {
        public string ProjectName = "Kouhai Project";
        public string ProjectDescription = "Enter your description here";
        public string Developer = "Kouhai";
        public string Version = "0.1";
        public string[] Tags;

        public bool DevelopementMode=false;
        public bool PackImages = true;
        public bool PackAudio = true;

        public static KouhaiPublishingData GetFromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            return JsonConvert.DeserializeObject<KouhaiPublishingData>(File.ReadAllText(path));
        }
    }
}