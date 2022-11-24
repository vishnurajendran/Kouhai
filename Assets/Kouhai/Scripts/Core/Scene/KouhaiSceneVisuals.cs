using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Core {
    public class KouhaiSceneVisuals : MonoBehaviour
    {
        private const string RES_BKG_IMG_PATH = "Images/Backgrounds/";

        [SerializeField]
        private Image background;

        public void ChangeBackground(string backgroundImageName)
        {
            background.sprite = Resources.Load<Sprite>($"{RES_BKG_IMG_PATH}{backgroundImageName}");
        }

        public string GetCurrent()
        {
            if(background.sprite != null)
            {
                return background.sprite.name;
            }
            else
            {
                return "";
            }
           
        }
    }
}
