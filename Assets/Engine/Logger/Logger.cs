using UnityEngine;
using EventType = Core.GameEvents.EventType;

namespace ExydeToolbox
{
    [System.Serializable]
     public class Logger
     {
         public static Logger Instance = null;
         
         [SerializeField] private LoggerSettings _settings;
         [SerializeField] private LoggerMode _loggerMode = 0;
         private void SetLoggerMode(LoggerMode mode) => _loggerMode = mode; 
         
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
             Instance = this;
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
                 Debug.Log(message.Color(_settings.EventColor));
             }
         }
         public void DebugLoggerState(){
             Debug.Log(_loggerMode);
         }
     }   
}


