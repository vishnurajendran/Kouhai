using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using Kouhai.Debugging;
using Kouhai.Core;

namespace Kouhai.Scripting.Interpretter
{
    public class KouhaiScript : MonoBehaviour
    {
        public enum ExecutionState
        {
            NOT_STARTED,
            RUNNING,
            WAITING
        }

        [SerializeField]
        private ExecutionState state;

        [SerializeField]
        internal KouhaiLuaScript source;
        internal Script luaScript;

        [SerializeField]
        internal List<string> vars;

        internal DynValue coroutine;
        bool IsInitialised
        {
            get
            {
                return luaScript != null;
            }
        }

        public void SetGlobal(string key, object global)
        {
            if (!IsInitialised)
                return;

            Debug.Log($"Setting {key}");
            luaScript.Globals[key] = global;
        }

        private void Start()
        {
            SetupScript();
        }

        private void SetupScript()
        {
            luaScript = new Script();
            vars = new List<string>();
            foreach (var global in luaScript.Globals.Keys)
            {
                vars.Add(global.ToString());
            }

            var func = Environment.KouhaiEnv.SetupScript(this, source.name);
            StartCoroutine(StartSceneLogic(func));
        }

        IEnumerator StartSceneLogic(DynValue func)
        {
            KouhaiDebug.Log("Scene Started");
            coroutine = luaScript.CreateCoroutine(func);
            coroutine.Coroutine.AutoYieldCounter = 1; //we yield after every frame.
            while (coroutine.Coroutine.State != CoroutineState.Dead)
            {
                //we stop execution to wait for player Input
                if (GlobalFlags.IsWaitingForPlayerInput)
                {
                    state = ExecutionState.WAITING;
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                state = ExecutionState.RUNNING;
                coroutine.Coroutine.Resume();
                yield return GlobalFlags.InterpreterWaitPeriod;
            }
            KouhaiDebug.Log("Scene Ended");
        }
    }
}
