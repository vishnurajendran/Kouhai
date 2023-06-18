using System;
using System.IO;
using System.Threading.Tasks;
using Kouhai.Core.AssetManagement;
using Kouhai.Runtime.System;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KouhaiStoryPack
{
    private StoryPackData packData;
    public string PackageTargetDirectory { get; private set; }
    public bool PackDataLoaded => packData != null;
    
    #if UNITY_EDITOR
    [MenuItem("Kouhai/Tests/Package Testing")]
    public static async void DoPackageTest()
    {
        var kpk = new KouhaiStoryPack();
        Debug.Log("LOAD TEST");
        await kpk.LoadPack($"{Application.persistentDataPath}/com.Kouhai.Kouhai Project.0_1_dev.khpak");
        await Task.Delay(3000);
        Debug.Log("UNLOAD TEST");
        await kpk.UnloadPack();
        Debug.Log("TEST COMPLETE");
    }
    #endif
    
    private void ValidateDirectory()
    {
        if (!Directory.Exists(PackageTargetDirectory))
            Directory.CreateDirectory(PackageTargetDirectory);
    }
    
    public KouhaiStoryPack()
    {
        PackageTargetDirectory = $"{Application.persistentDataPath}/Kouhai_temp_{Guid.NewGuid()}";
        ValidateDirectory();
    }
    
    public async Task LoadPack(string pakName)
    {
        if (packData != null)
            UnloadPack();

        if (string.IsNullOrEmpty(pakName))
        {
            return;
        };

        var path = KouhaiUserDataPaths.GetGameDirectory(pakName);
        if (!Directory.Exists(path))
            return;

        ValidateDirectory();
        packData = await StoryPackData.LoadPack(path,PackageTargetDirectory );
    }
    
    public async Task UnloadPack()
    {
        if (packData != null)
        {
            await packData.Unload();
        }
        Directory.Delete(PackageTargetDirectory, true);
        packData = null;
    }
    
    public T LoadAsset<T>(string path) where T : Object
    {
        if (packData == null)
        {
            Debug.LogError("No Kouhai Story Packs were loaded!");
            return null;
        }
        return packData.LoadAsset<T>(path);
    }
    
    public T[] LoadAssetAll<T>(string path) where T : Object
    {
        if (packData == null)
        {
            Debug.LogError("No Kouhai Story Packs were loaded!");
            return null;
        }
        return packData.LoadAssetAll<T>(path);
    }
}
