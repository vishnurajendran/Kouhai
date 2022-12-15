using Newtonsoft.Json;
using UnityEngine;
using RuntimeDeveloperConsole;
using System.Linq;

namespace Kouhai.Core.Input {
    public class KouhaiInputSystem : MonoBehaviour
    {
        [System.Serializable]
        private enum KeyInputTypes
        {
            SELECT,
            CANCEL,
            NAVIGATE_UP,
            NAVIGATE_DOWN,
        }

        private const string CONFIG_FILE = "InputConfig.json";

        private bool BlockInputs { get; set; } = false;
        private static string FilePath => $"{Application.dataPath}/{CONFIG_FILE}";
        
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
        private IConsoleWindow consoleWindow;
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

        private void Start()
        {
            var windows = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IConsoleWindow>();
            consoleWindow = windows.FirstOrDefault();
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

        private void LoadInputConfig()
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

        private void Update()
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
            if (BlockInputs || (consoleWindow != null  && consoleWindow.IsOpen))
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