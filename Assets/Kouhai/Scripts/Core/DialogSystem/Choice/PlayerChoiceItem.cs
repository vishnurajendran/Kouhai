using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Core {
    public class PlayerChoiceItem : MonoBehaviour
    {
        private Button bttn;
        private TMPro.TMP_Text choiceString;
        private PlayerChoiceSystem system;
        private int id;

        public void Init(PlayerChoiceSystem system, string choiceStr,int id)
        {
            this.system = system;
            this.id = id;
            bttn = GetComponentInChildren<Button>();
            bttn.onClick.AddListener(OnChoiceClicked);

            this.choiceString = GetComponentInChildren<TMPro.TMP_Text>();
            this.choiceString.text = choiceStr;
        }

        private void OnChoiceClicked()
        {
            system.SetPlayerChoice(id);
        }

        public void OnMouseEnter()
        {
            this.transform.localScale = Vector3.one * 1.15f;
        }

        public void OnMouseExit()
        {
            this.transform.localScale = Vector3.one;
        }
    }
}
