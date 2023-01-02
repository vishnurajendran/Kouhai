using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using Kouhai.Debugging;
using Kouhai.Core;

namespace Kouhai.Scripting.Interpretter
{
    public class KouhaiScript : MonoBehaviour
    {
        private enum ExecutionState
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
        private DynValue coroutine;
        private bool IsInitialised => luaScript != null;

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

        private void SetupScript()
        {
            luaScript = new Script();
            var func = Environment.KouhaiEnv.SetupScript(this, source.name);
            StartCoroutine(StartSceneLogic(func));
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