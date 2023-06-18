using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Kouhai.Core.AssetManagement
{
    public interface IAssetList
    {
        public Type TargetType { get; }
        public Task Initialise(string assetDir, Dictionary<string, string> assetMap);
        public Object GetAsset(string path);
        public Object[] GetAssetAll(string path);
        public Task Cleanup();
    }

}
