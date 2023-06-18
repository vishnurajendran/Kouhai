using Kouhai.Publishing;

namespace Kouhai.Scripts.Runtime.System.Settings
{
    public static class KouhaiAppSettings
    {
        public static KouhaiAppLocalisation.Languange Languange => KouhaiAppSettingsData.Settings.SelectedLanguage;
        
        public static float MasterVolume => KouhaiAppSettingsData.Settings.MasterVolumeLevel;
        public static float BackgrounVolume => KouhaiAppSettingsData.Settings.BackgroundVolumeLevel;
        public static float SfxVolume => KouhaiAppSettingsData.Settings.SfxVolumeLevel;
    }
}