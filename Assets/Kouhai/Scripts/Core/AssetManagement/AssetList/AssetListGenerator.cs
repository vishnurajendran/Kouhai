using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public static class AssetListGenerator
    {
        public static IAssetList GetSpriteAssetList()
        {
            return new SpriteAssetList();
        }
        
        public static IAssetList GetAudioAssetList()
        {
            return new AudioAssetList();
        }
        
        public static IAssetList GetScriptAssetList()
        {
            return new ScriptAssetList();
        }
    }
}