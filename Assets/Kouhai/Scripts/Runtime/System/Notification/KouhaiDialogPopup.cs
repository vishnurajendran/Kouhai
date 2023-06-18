using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kouhai.Scripts.Runtime.System.Notification
{
    public class KouhaiDialogPopup : MonoBehaviour
    {
        public enum DialogType
        {
            INFO,
            WARNING,
            ERROR
        }

        [Header("Error Color")]
        [SerializeField] private Color errorBackground;
        [SerializeField] private Color errorText;
        
        [Header("Warning Color")]
        [SerializeField] private Color warningBackground;
        [SerializeField] private Color warningText;
        
        [Header("Normal Color")]
        [SerializeField] private Color normalBacground;
        [SerializeField] private Color normalText;
        
        [Header("References")]
        [SerializeField] private Transform popupDialogParent;
        [SerializeField] private Image background;
        [SerializeField] private Transform okParent;
        [SerializeField] private Transform yesnoParent;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text desc;
        [SerializeField] private Button okButton;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        public void SetType(DialogType type)
        {
            switch (type)
            {
                case DialogType.INFO:
                    SetNormal();
                    break;
                
                case DialogType.WARNING:
                    SetWarning();
                    break;
                
                case DialogType.ERROR:
                    SetError();
                    break;
            }
        }

        private void SetNormal()
        {
            SetTextColor(normalText);
            SetBackgroundColor(normalBacground);
        }
        
        private void SetWarning()
        {
            SetTextColor(warningText);
            SetBackgroundColor(warningBackground);
        }
        
        private void SetError()
        {
            SetTextColor(errorText);
            SetBackgroundColor(errorBackground);
        }

        private void SetTextColor(Color color)
        {
            title.color = color;
            desc.color = color;
            okButton.GetComponentInChildren<TMP_Text>().color = color;
            yesButton.GetComponentInChildren<TMP_Text>().color = color;
            noButton.GetComponentInChildren<TMP_Text>().color = color;
        }

        private void SetBackgroundColor(Color col)
        {
            background.color = col;
        }
        
        public void SetupOKDialog(string title, string desc, string ok, Action onClick)
        {
            okParent.gameObject.SetActive(true);
            yesnoParent.gameObject.SetActive(false);
            
            this.title.text = title;
            this.desc.text = desc;
            okButton.GetComponentInChildren<TMP_Text>().text = ok;
            okButton.onClick.AddListener(()=>
            {
                onClick?.Invoke();
                CloseDialog();
            });
            StartCoroutine(Pop(true, 0.25f));
        }
        
        public void SetupYesNoDialog(string title, string desc, string yes, string no, Action onClickYes, Action onClickNo)
        {
            okParent.gameObject.SetActive(false);
            yesnoParent.gameObject.SetActive(true);
            
            this.title.text = title;
            this.desc.text = desc;
            yesButton.GetComponentInChildren<TMP_Text>().text = yes;
            yesButton.onClick.AddListener(()=>
            {
                onClickYes?.Invoke();
                CloseDialog();
            });
            
            noButton.GetComponentInChildren<TMP_Text>().text = no;
            noButton.onClick.AddListener(()=>
            {
                onClickNo?.Invoke();
                CloseDialog();
            });

            StartCoroutine(Pop(true, 0.25f));
        }

        private void CloseDialog()
        {
            StartCoroutine(Pop(false, 0.25f, () =>
            {
                Destroy(this.gameObject);
            }));
        }

        IEnumerator Pop(bool popin, float time, Action onComplete=null)
        {
            float timestep = 0;
            var currSize = popupDialogParent.localScale;
            var final = popin ? Vector3.zero : Vector3.one;
            while (timestep <= 1)
            {
                timestep += Time.deltaTime /time;
                popupDialogParent.localScale = Vector3.Lerp(currSize, final, timestep);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }
    }
}