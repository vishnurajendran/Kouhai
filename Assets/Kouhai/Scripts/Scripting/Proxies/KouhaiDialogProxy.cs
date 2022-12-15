using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;

namespace Kouhai.Scripting.Proxies
{
    [MoonSharpUserData]
    public class KouhaiDialogProxy : KouhaiRuntimeProxy
    {
        private Core.DialogUI dialogSystem;
        private Core.PlayerChoiceSystem choiceSystem;

        [MoonSharpHidden]
        public override string Symbol => "Dialog";
        [MoonSharpHidden]
        public override KouhaiRuntimeProxy GetProxyInstance()
        {
            return new KouhaiDialogProxy();
        }

        [MoonSharpHidden]
        public KouhaiDialogProxy()
        {
            this.dialogSystem = GameObject.FindObjectOfType<Core.DialogUI>();
            this.choiceSystem = GameObject.FindObjectOfType<Core.PlayerChoiceSystem>();
        }

        public Table Say
        {
            get => null;
            set
            {
                var speaker = value.Get(1).String;
                var speech = value.Get(2).String;
                dialogSystem.SayDialog(speech);
            }
        }

        public Table Choices
        {
            get =>null;
            set
            {
                var list = new List<string>();
                for(int i = 1; i <= value.Length; i++)
                {
                    list.Add(value.Get(i).String);
                }

                choiceSystem.SetChoices(list);
            }
        }
        
        public int PlayerChoice=> choiceSystem.PlayerChoice;
    }
}