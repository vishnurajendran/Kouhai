using System;
using Kouhai.Runtime.Client;

namespace Kouhai.Runtime.System
{
    public static class KouhaiResourcesPath
    {
        public const string LoadingScreenPath = "Prefabs/System/LoadingScreen";
        public const string PopupDialog = "Prefabs/System/PopupDialog";
        public const string KouhaiSystem = "Prefabs/System/KouhaiSystems";
        public const string GameDetailsParent = "Prefabs/AppClient/HomeScreen/GameDetailsParent";
        public const string GameDetails = "Prefabs/AppClient/HomeScreen/GameDetails";
        
        public const string LibraryItem = "Prefabs/AppClient/HomeScreen/LibraryItem";
        
        private const string LibraryPage = "Prefabs/AppClient/HomeScreen/Pages/LibraryPage";
        private const string StorePage = "Prefabs/AppClient/HomeScreen/Pages/StorePage";
        private const string DownloadsPage = "Prefabs/AppClient/HomeScreen/Pages/DownloadsPage";
        private const string SettingsPage = "Prefabs/AppClient/HomeScreen/Pages/SettingsPage";
        
        
        public static string GetLocalisationFilePath(KouhaiAppLocalisation.Languange language)
        {
            return $"Localisation/localisation_map_{language.ToString().ToLower()}";
        }

        public static string GetHomeTabPagePath(KouhaiHomescreen.HomeScreenTab targetTab)
        {
            switch (targetTab)
            {
                case KouhaiHomescreen.HomeScreenTab.Library:
                    return LibraryPage;
                case KouhaiHomescreen.HomeScreenTab.Store:
                    return StorePage;
                case KouhaiHomescreen.HomeScreenTab.Downloads:
                    return DownloadsPage;
                case KouhaiHomescreen.HomeScreenTab.Settings:
                    return SettingsPage;
                
                default:
                    return String.Empty;
            }
        }
    }   
}
