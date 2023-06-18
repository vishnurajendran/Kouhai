using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Runtime.System
{
    public static class KouhaiResourcesPath
    {
        public const string LoadingScreenPath = "Prefabs/LoadingScreen";
        public const string PopupDialog = "Prefabs/PopupDialog";
        public const string KouhaiSystem = "Prefabs/KouhaiSystems";
        
        public static string GetLocalisationFilePath(KouhaiAppLocalisation.Languange language)
        {
            return $"Localisation/localisation_map_{language.ToString().ToLower()}";
        } 
    }   
}
