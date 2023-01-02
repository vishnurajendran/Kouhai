using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Core
{
    public static class KouhaiGlobals
    {
        //Consts
        public const string SCRIPT_WAIT_TIME_KEY = "_KHLS_SCRIPT_EXEC_HALT";
        
        /// <summary>
        /// Text dialog fade duration
        /// </summary>
        public const float FadeDuration = 0.5f;
        /// <summary>
        /// Animation duration for Text animation in dialog box
        /// </summary>
        public const float TextAnimationDuration = 0.025f;

        //Properties
        /// <summary>
        /// Set to true, when kouhai is waiting for PlayerInput
        /// </summary>
        public static bool IsWaitingForPlayerInput { get; set; }
        /// <summary>
        /// Set to true to block lua execution,
        /// ideally this should be reset when no longer needed
        /// </summary>
        public static bool BlockLuaExecution { get; set; }
        
        /// <summary>
        /// Wait period between executing two consecutive lua statetments
        /// </summary>
        public static UnityEngine.Object DelayBetweenStatementsExecution => null;
    }
}
