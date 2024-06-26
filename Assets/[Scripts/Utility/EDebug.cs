﻿using UnityEngine;

namespace Utility
{
    /**
     * A class that only executes in the Unity editor
     */
    public static class EDebug
    {
        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.Log(message, context);
#endif
        }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void ColorLog(object message, Color color)
        {
#if UNITY_EDITOR
          EDebug.Log( StringUtil.addColorToString(message.ToString(), color) );
#endif
        }
    }
}