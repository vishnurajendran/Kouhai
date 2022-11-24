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
        get
        {
            return null;
        }

        set
        {
            var name = value.Get(1).String;
            var expression = value.Get(2).String;
            var position = value.Get(3).String;
            characterSystem.ShowCharacter(name, expression, position);
        }
    }
}
