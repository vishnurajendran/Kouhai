using System;
using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using Kouhai.Debugging;
using Kouhai.Core;
using Kouhai.Scripting.Environment;

namespace Kouhai.Scripting.Interpretter
{
    [RequireComponent(typeof(KouhaiVariables))]
    public class KouhaiScript : MonoBehaviour
    {
        private enum ExecutionState
        {
            NOT_STARTED,
            RUNNING,
            WAITING
        }
        private ExecutionState state;
        [SerializeField]
        internal KouhaiLuaScript source;
        internal Script luaScript;
        private DynValue coroutine;
        private bool IsInitialised => luaScript != null;

        private KouhaiVariables variables;

        private DynValue compiledScriptFunc;
        
        public void SetGlobal(string key, object global)
        {
            if (!IsInitialised)
                return;
            
            luaScript.Globals.Set(key, DynValue.FromObject(luaScript,global));
        }

        public void RemoveGlobal(string key)
        {
            if (!IsInitialised)
                return;

            luaScript.Globals.Remove(key);
        }

        public DynValue GetGlobal(string key)
        {
            if(!IsInitialised)
                return DynValue.Nil;
            
            return luaScript.Globals.Get(key);
        }
        
        private void Start()
        {
            SetupScript();
        }

        public void SetSource(KouhaiLuaScript source)
        {
            this.source = source;
        }
        
        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            
            if (variables == null)
                variables = GetComponent<KouhaiVariables>();

            if (source == null)
            {
                variables.Clear();
                return;
            }
            
            KouhaiEnv.RequireRecompile -= TryCompile;
            KouhaiEnv.RequireRecompile += TryCompile;

            TryCompile();
        }

        private void TryCompile()
        {
            SetupScript(true);
            luaScript.Call(compiledScriptFunc);
            variables.Load(luaScript);
        }
        
        private void SetupScript(bool loadOnly=false)
        {
            if(source == null)
                return;
            
            luaScript = new Script();
            compiledScriptFunc = Environment.KouhaiEnv.SetupScript(this, source.name);
            if(!loadOnly)
                StartCoroutine(StartSceneLogic(compiledScriptFunc));
        }

        private IEnumerator StartSceneLogic(DynValue func)
        {
            //KouhaiDebugger.AttachDebugger(this);
            KouhaiDebug.Log("Scene Started");
            coroutine = luaScript.CreateCoroutine(func);
            coroutine.Coroutine.AutoYieldCounter = 1; //we yield after every frame.
            while (coroutine.Coroutine.State != CoroutineState.Dead)
            {
                //we stop execution to wait for player Input
                if (KouhaiGlobals.IsWaitingForPlayerInput)
                {
                    state = ExecutionState.WAITING;
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                
                // Waiting for some seconds before executing next statmement.
                var executionWait = GetGlobal(KouhaiGlobals.SCRIPT_WAIT_TIME_KEY);
                if (executionWait != DynValue.Nil)
                {
                    state = ExecutionState.WAITING;
                    RemoveGlobal(KouhaiGlobals.SCRIPT_WAIT_TIME_KEY);
                    yield return new WaitForSeconds((float)executionWait.Number);
                }
                
                state = ExecutionState.RUNNING;
                coroutine.Coroutine.Resume();
                yield return KouhaiGlobals.DelayBetweenStatementsExecution;
            }
            KouhaiDebug.Log("Scene Ended");
        }
    }
}