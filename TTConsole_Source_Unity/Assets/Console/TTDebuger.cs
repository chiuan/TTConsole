namespace TinyTeam.Debuger
{
    /// <summary>
    /// Description： custom debuger,can control debug or record in memory writing in thread.
    /// Author： chiuanwei
    /// CreateTime: 2015-2-12
    /// </summary>


    using System.Collections;
    using System;

#if UNITY_ENGINE
    using UnityEngine;
#endif

    public class TTDebuger
    {
        static public bool EnableLog = true;
        static public void Log(object message)
        {
            Log(message, null);
        }

        static public void Log(object message, string customType)
        {
            if (EnableLog)
            {
#if UNITY_ENGINE
                Console.Log(message,customType);
#else
                System.Console.WriteLine("Log>" + customType + " : " + message);
#endif
            }
        }

#if UNITY_ENGINE
        // Dont use Color..

        //static public void Log(object message, string customType,Color col)
        //{
        //    if (EnableLog)
        //    {
        //        Console.Log(message, customType, col);
        //    }
        //}
#endif

        static public void LogError(object message)
        {
            LogError(message, null);
        }

        static public void LogError(object message, string customType)
        {
            if (EnableLog)
            {
#if UNITY_ENGINE
            Console.LogError(message, customType);
#else
            System.Console.WriteLine("LogError>" + customType + " : " + message);
#endif
            }
        }
        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        static public void LogWarning(object message, string customType)
        {
            if (EnableLog)
            {
#if UNITY_ENGINE
                Console.LogWarning(message, customType);
#else
                System.Console.WriteLine("LogWarning>" + customType + " : " + message);
#endif
            }
        }

        public static void RegisterCommand(string commandString, Func<string[],object> commandCallback, string CMD_Discribes)
        {
            if (EnableLog)
            {
#if UNITY_ENGINE
                Console.RegisterCommand(commandString, commandCallback, CMD_Discribes);
#endif
            }
        }

        public static void UnRegisterCommand(string commandString)
        {
            if (EnableLog)
            {
#if UNITY_ENGINE
                Console.UnRegisterCommand(commandString);
#endif
            }
        }

    }
}