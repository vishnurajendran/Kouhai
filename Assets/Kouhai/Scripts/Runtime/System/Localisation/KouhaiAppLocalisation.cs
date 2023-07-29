using System.Collections;
using System.Collections.Generic;
using Kouhai.Runtime.System;
using Kouhai.Scripts.Runtime.System.Settings;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public class KouhaiAppLocalisation
{
    public enum Languange
    {
        ENGLISH = 0
    }
    public enum LocalisationTextType
    {
        BootLoadingScreen_1,
        BootLoadingScreen_2,
        ErrorEncountered,
        BootLoadingScreenStoryPackErr,
        EntryPointMissing,
        DeleteGameTitle,
        DeleteGameDesc,
        Library,
        Store,
        Settings,
        Downloads,
        Ok,
        Yes,
        No
    }

    private Dictionary<LocalisationTextType, string> localisationMap;

    private static KouhaiAppLocalisation current;
    public static KouhaiAppLocalisation Current
    {
        get
        {
            if (current == null)
            {
                current = new KouhaiAppLocalisation(KouhaiAppSettings.Languange);
            }

            return current;
        }
    }

    public KouhaiAppLocalisation(Languange language)
    {
        var asset = Resources.Load<TextAsset>(KouhaiResourcesPath.GetLocalisationFilePath(language));
        if (asset == null)
            localisationMap = null;
        else
        {
            localisationMap = JsonConvert.DeserializeObject<Dictionary<LocalisationTextType, string>>(asset.text);
        }
    }

    public string GetLocalisedText(LocalisationTextType type)
    {
        if (localisationMap == null || !localisationMap.ContainsKey(type))
            return string.Empty;

        return localisationMap[type];
    }
}
