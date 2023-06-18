using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public class KouhaiAssetManager : MonoBehaviour
    {
        private static KouhaiStoryPack activeStoryPack;
        public static async void LoadStoryPack(string pakName, Action<bool> onComplete)
        {
            activeStoryPack = new KouhaiStoryPack();
            await activeStoryPack.LoadPack(pakName);
            if (activeStoryPack.PackDataLoaded)
                onComplete?.Invoke(true);
            else
                onComplete?.Invoke(false);
        }

        public static async void UnloadStoryPack(Action onComplete)
        {
            if (activeStoryPack != null && activeStoryPack.PackDataLoaded)
            {
                await activeStoryPack.UnloadPack();
                activeStoryPack = null;
            }
            onComplete?.Invoke();
        }
        
        public static T LoadAsset<T>(string path) where T : Object
        {
#if UNITY_EDITOR && !KOUHAI_APP_TESTING
            return Resources.Load<T>(path);
#else
            T asset = null;
            if(activeStoryPack != null && activeStoryPack.PackDataLoaded)
                asset = activeStoryPack.LoadAsset<T>(path);
            if (asset == null)
                return null;
            return asset;
#endif
        }
        
        public static T[] LoadAssetAll<T>(string path) where T : Object
        {
#if UNITY_EDITOR && !KOUHAI_APP_TESTING
            return Resources.LoadAll<T>(path);
#else
            T[] res = null;
            if(activeStoryPack != null && activeStoryPack.PackDataLoaded)
                res = activeStoryPack.LoadAssetAll<T>(path);
            return res;
#endif
        }
    }

}
