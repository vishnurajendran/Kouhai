using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Debugging
{
    public static class KouhaiDebug
    {
        private const string KOUHAI_DEBUG_SYMBOL = "KouhaiDebug::";

        public static void Log(string logMsg)
        {
            Debug.Log($"{KOUHAI_DEBUG_SYMBOL}{logMsg}");
        }

        public static void LogColor(string logMsg, Color color)
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{KOUHAI_DEBUG_SYMBOL}{logMsg}</color>");
        }

        public static void LogError(string err)
        {
            Debug.LogError($"{KOUHAI_DEBUG_SYMBOL}{err}");
        }

        public static void LogWarning(string warning)
        {
            Debug.LogWarning($"{KOUHAI_DEBUG_SYMBOL}{warning}");
        }
        
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
