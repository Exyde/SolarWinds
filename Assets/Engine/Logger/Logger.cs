using UnityEngine;
using Core.GameEvents;
using EventType = Core.GameEvents.EventType;

public static class Logger{

    [System.Flags]
    public enum LoggerMode { 
        Errors = 1,
        Warning = 2,
        Dialogue = 4,
        Info = 8,
        Events = 16
    }
    private static LoggerMode _loggerMode = 0;
    public static void SetLoggerMode(LoggerMode mode) => _loggerMode = mode; 

    //Colors - Add Settingd
    private static readonly string _infoColor = "cyan";
    private static readonly string _eventColor = "purple";

    public static void Log (object message) => Debug.Log(message);

    public static void LogError(object message){
        if (_loggerMode.HasFlag(LoggerMode.Errors))
            Debug.LogError(message);
    }

    public static void LogWarning(object message){
        if (_loggerMode.HasFlag(LoggerMode.Warning))
            Debug.LogWarning(message);
    }

    public static void LogDialogue(){
        if (_loggerMode.HasFlag(LoggerMode.Dialogue))
            return;
    }

    public static void LogInfo(object message){
        if (_loggerMode.HasFlag(LoggerMode.Info))
            Debug.Log(message.ToString().Color(_infoColor));
    }

    public static void LogEvent(EventType eventType, string eventSender, string managerName){
        if (_loggerMode.HasFlag(LoggerMode.Events)){

            string message = $"[{managerName}] : Handling {eventType} sent by {eventSender}...";
            Debug.Log(message.ToString().Color(_eventColor));
        }
    }

    public static void DebugLoggerState(){
        Debug.Log(_loggerMode);
    }
}

