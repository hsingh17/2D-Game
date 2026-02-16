using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logger
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public static void Log(
        string message,
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = -1
    )
    {
        Debug.Log($"{message}.\n{callerFilePath}:{callerLineNumber}");
    }

    public static void LogError(
        string message,
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = -1
    )
    {
        Debug.Log($"{message}.\n{callerFilePath}:{callerLineNumber}");
    }

    public static void LogWarning(
        string message,
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = -1
    )
    {
        Debug.Log($"{message}.\n{callerFilePath}:{callerLineNumber}");
    }

#endif
}
