using Kouhai.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Core
{
    public class GlobalFlags
    {
        //Consts
        public const float FadeDuration = 0.5f;
        public const float TextAnimationDuration = 0.025f;

        //Properties
        public static bool IsWaitingForPlayerInput { get; set; }

        public static UnityEngine.Object InterpreterWaitPeriod
        {
            get { return null; }
        }

    }
}
