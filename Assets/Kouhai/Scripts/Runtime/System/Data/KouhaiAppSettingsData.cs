using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Kouhai.Publishing
{
    public class KouhaiAppSettingsData
    {
        private static string SettingsFilePath => $"{Application.persistentDataPath}/.appsettings";
        private static KouhaiAppSettingsData setingsInstance;

        public static KouhaiAppSettingsData Settings
        {
            get
            {
                if (setingsInstance == null)
                    setingsInstance = LoadInstance();

                return setingsInstance;
            }
        }
        
        private static void SaveInstance(KouhaiAppSettingsData instance)
        {
            File.WriteAllText(SettingsFilePath,JsonConvert.SerializeObject(instance));
        }

        private static KouhaiAppSettingsData LoadInstance()
        {
            
            KouhaiAppSettingsData settingsInstance = null;
            if (File.Exists(SettingsFilePath))
            {
                settingsInstance = JsonConvert.DeserializeObject<KouhaiAppSettingsData>(File.ReadAllText(SettingsFilePath));
            }
            else
            {
                settingsInstance = new KouhaiAppSettingsData();
                SaveInstance(settingsInstance);
            }
            return settingsInstance;
        }
        
        #region Language setting
        /// <summary>
        /// Language selected by user
        /// </summary>
        public KouhaiAppLocalisation.Languange SelectedLanguage;
        #endregion
        
        #region Volume Settings
        /// <summary>
        /// Master volume (%)
        /// </summary>
        public float MasterVolumeLevel = 1;
        /// <summary>
        /// SFX volume (%)
        /// </summary>
        public float SfxVolumeLevel = 1;
        /// <summary>
        /// Background volume (%)
        /// </summary>
        public float BackgroundVolumeLevel = 1;
        #endregion
    }
}