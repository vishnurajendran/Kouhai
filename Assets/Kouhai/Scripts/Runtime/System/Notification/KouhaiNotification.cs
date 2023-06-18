using System;
using Kouhai.Runtime.System;
using UnityEngine;

namespace Kouhai.Scripts.Runtime.System.Notification
{
    public static class KouhaiNotificationManager
    {
        public static void ShowDialogOk(KouhaiDialogPopup.DialogType type, string title, string desc, string ok, Action onClick)
        {
            var instance = SpawnDialogOk();
            instance.SetType(type);
            instance.SetupOKDialog(title, desc, ok, onClick);
        }
    
        public static void ShowDialogYesNo(KouhaiDialogPopup.DialogType type, string title, string desc, string yes,string no, Action onClickYes, Action onClickNo)
        {
            var instance = SpawnDialogYesNo();
            instance.SetType(type);
            instance.SetupYesNoDialog(title, desc, yes, no, onClickYes, onClickNo);
        }

        private static KouhaiDialogPopup SpawnDialogOk()
        {
            var instance = GameObject.Instantiate(LoadOkDialog());
            return instance.GetComponent<KouhaiDialogPopup>();
        }
    
        private static KouhaiDialogPopup SpawnDialogYesNo()
        {
            var instance = GameObject.Instantiate(LoadYesNoDialog());
            return instance.GetComponent<KouhaiDialogPopup>();
        }
    
        private static GameObject LoadOkDialog()
        {
            return Resources.Load<GameObject>(KouhaiResourcesPath.PopupDialog);
        }
    
        private static GameObject LoadYesNoDialog()
        {
            return Resources.Load<GameObject>(KouhaiResourcesPath.PopupDialog);
        }
    }
}

