using UnityEngine;
using Core.GameEvents;
using EventType = Core.GameEvents.EventType;

namespace ExydeToolbox
{
     public class Logger
     {
         private LoggerSettings _settings;
         private static LoggerMode _loggerMode = 0;
         private static void SetLoggerMode(LoggerMode mode) => _loggerMode = mode; 
         
         [System.Flags]
         public enum LoggerMode { 
             Errors = 1,
             Warning = 2,
             Dialogue = 4,
             Info = 8,
             Events = 16
         }
     
         public Logger(LoggerSettings settings)
         {
             _settings = settings;
             SetLoggerMode(settings.Mode);
         }
        public  void Log (object message) => Debug.Log(message);
         public  void LogError(object message){
             if (_loggerMode.HasFlag(LoggerMode.Errors))
                 Debug.LogError(message);
         }
         public  void LogWarning(object message){
             if (_loggerMode.HasFlag(LoggerMode.Warning))
                 Debug.LogWarning(message);
         }
         public  void LogDialogue(object message)
         {
             if (_loggerMode.HasFlag(LoggerMode.Dialogue))
                 Debug.Log(message);
         }
         public  void LogInfo(object message){
             if (_loggerMode.HasFlag(LoggerMode.Info))
                 Debug.Log(message.ToString().Color(_settings.InfoColor));
         }
         public  void LogEvent(EventType eventType, string eventSender, string managerName){
             if (_loggerMode.HasFlag(LoggerMode.Events)){
                 var message = $"[{managerName}] : Handling {eventType} sent by {eventSender}...";
                 Debug.Log(message.ToString().Color(_settings.EventColor));
             }
         }
         public void DebugLoggerState(){
             Debug.Log(_loggerMode);
         }
     }   
}


