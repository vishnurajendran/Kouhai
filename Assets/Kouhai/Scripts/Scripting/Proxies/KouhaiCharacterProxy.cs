using Kouhai.Scripting.Interpretter;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MoonSharpUserData]
public class KouhaiCharacterProxy : KouhaiRuntimeProxy
{
    private KouhaiCharacterHandler characterSystem;

    public override string Symbol => "Character";

    public override KouhaiRuntimeProxy GetProxyInstance()
    {
        return new KouhaiCharacterProxy();
    }

    public KouhaiCharacterProxy()
    {
        characterSystem = GameObject.FindObjectOfType<KouhaiCharacterHandler>();
    }

    public Table Show
    {
        get => null;
        set
        {
            var name = value.Get(1).String;
            var expression = value.Get(2).String;
            var position = value.Get(3).String;
            characterSystem.ShowCharacter(name, expression, position);
        }
    }

    public void HideCharacter(string name)
    {
        characterSystem.HideCharacter(name);
    }

    public void ShiftCharacter (Table dataTable) {
        characterSystem.ShiftCharacter(dataTable.Get(1).String, dataTable.Get(2).String, (float)dataTable.Get(3).Number);
    }
}
