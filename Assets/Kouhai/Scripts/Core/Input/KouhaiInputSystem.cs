using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Kouhai.Core.Input {
    public class KouhaiInputSystem : MonoBehaviour
    {
        public enum KeyInputTypes
        {
            SELECT,
            CANCEL,
            NAVIGATE_UP,
            NAVIGATE_DOWN,
        }

        private const string CONFIG_FILE = "InputConfig.json";

        public bool BlockInputs { get; set; } = false;

        /// <summary>
        /// User presses select key
        /// </summary>
        public System.Action OnUserSelects;
        /// <summary>
        /// User presses cancel key
        /// </summary>
        public System.Action OnUserCancels;

        /// <summary>
        /// User presses navigation up
        /// </summary>
        public System.Action OnUserNaviagtesUp;

        /// <summary>
        /// User presses navigation up
        /// </summary>
        public System.Action OnUserNaviagtesDown;

        [SerializeField]
        private SerializableDictionary<KeyInputTypes, KeyCode> keyConfig; 

        private static string FilePath
        {
            get
            {
                return $"{Application.dataPath}/{CONFIG_FILE}";
            }
        }

        private static SerializableDictionary<KeyInputTypes,KeyCode> SavedConfigData
        {
            get
            {
                Debug.Log("Loaded saved config");
                return JsonConvert.DeserializeObject<SerializableDictionary<KeyInputTypes, KeyCode>>(System.IO.File.ReadAllText(FilePath));
            }
        }

        private static KouhaiInputSystem instance;
        public static KouhaiInputSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<KouhaiInputSystem>();
                    if (instance == null)
                    {
                        instance = new GameObject("KouhaiInputSystem").AddComponent<KouhaiInputSystem>();
                    }


                    instance.LoadInputConfig();
                    DontDestroyOnLoad(instance.gameObject);
                } 
                return instance;
            }
        }

        private SerializableDictionary<KeyInputTypes, KeyCode> GetDefaultControlScheme()
        {
            return new SerializableDictionary<KeyInputTypes, KeyCode>()
            {
                { KeyInputTypes.SELECT, KeyCode.Return },
                { KeyInputTypes.CANCEL, KeyCode.Escape },
                { KeyInputTypes.NAVIGATE_UP, KeyCode.UpArrow },
                { KeyInputTypes.NAVIGATE_DOWN, KeyCode.DownArrow }
            };
        }

        public void LoadInputConfig()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                Debug.Log("Loading Default Control scheme");
                keyConfig = GetDefaultControlScheme();
                System.IO.File.WriteAllText(FilePath, JsonConvert.SerializeObject(keyConfig));
                return;
            }

            keyConfig = SavedConfigData;
        }

        void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                CheckForInputs();
            }
        }

        private void CheckForInputs()
        {
            foreach(var kv in keyConfig)
            {
                if (UnityEngine.Input.GetKeyDown(kv.Value))
                {
                    EmitEvent(kv.Key);
                }
            }
        }

        private void EmitEvent(KeyInputTypes type)
        {
            if (BlockInputs)
                return;

            switch (type)
            {
                case KeyInputTypes.SELECT:
                    OnUserSelects?.Invoke();
                    break;
                case KeyInputTypes.CANCEL:
                    OnUserCancels?.Invoke();
                    break;
                case KeyInputTypes.NAVIGATE_UP:
                    OnUserNaviagtesUp?.Invoke();
                    break;
                case KeyInputTypes.NAVIGATE_DOWN:
                    OnUserNaviagtesDown?.Invoke();
                    break;
            }
        }
    }
}