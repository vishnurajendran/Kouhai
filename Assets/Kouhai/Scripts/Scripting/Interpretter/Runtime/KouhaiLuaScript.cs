using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    public class KouhaiLuaScript : ScriptableObject
    {
        [SerializeField]
        private string _source;

        public string Source => _source;

        public void Initialise(string text)
        {
            this._source = text;
        }
    }
}
