using UnityEngine;

namespace Kouhai.Runtime.System
{
    public static class KouhaiUserDataPaths
    {
        public static string GetGameDirectory(string pakName)
        {
            return $"{Application.persistentDataPath}/UserLibrary/{pakName}";
        }
    }
}