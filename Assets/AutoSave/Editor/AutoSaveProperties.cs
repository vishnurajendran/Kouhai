using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoSaver {
    public class AutoSaveProperties : ScriptableObject
    {
        private const string PROP_ASSET_PATH = "Assets/AutoSaveProperties.asset";

        [Tooltip("Log every auto save?")]
        public bool Log = false;

        [Tooltip("Start on Editor Load?")]
        public bool StartOnEditorLoad = true;

        [Tooltip("How many seconds before Autosave kicks in?")]
        public int TimeBetweenAutoSaves = 30;

        private static AutoSaveProperties properties;
        public static AutoSaveProperties Properties
        {
            get
            {
                if (properties == null)
                    properties = LoadOrCreateProperties();

                return properties;
            }
        }

        private static AutoSaveProperties LoadOrCreateProperties()
        {
            var propRef = AssetDatabase.LoadAssetAtPath<AutoSaveProperties>(PROP_ASSET_PATH);
            if (propRef == null) {
                propRef = ScriptableObject.CreateInstance<AutoSaveProperties>();
                AssetDatabase.CreateAsset(propRef, PROP_ASSET_PATH);
            }

            return propRef;
        }
    }
}
