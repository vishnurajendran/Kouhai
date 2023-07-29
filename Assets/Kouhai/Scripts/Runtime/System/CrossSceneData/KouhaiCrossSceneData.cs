using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Runtime.System
{
    public class KouhaiCrossSceneData : MonoBehaviour
    {
        private static KouhaiCrossSceneData instance;

        public static KouhaiCrossSceneData Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("KouhaiCrossSceneData");
                    instance = go.AddComponent<KouhaiCrossSceneData>();
                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
   
        public string SelectedGame { get; private set; }
        public void SetSelectedGameDirectory(string gamePath)
        {
            SelectedGame = gamePath;
        }
   
    }
}

