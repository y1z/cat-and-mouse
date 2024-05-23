using UnityEngine;

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
    }
}