using UnityEngine;
using Core.GameEvents;

public class LoggerConfig : MonoBehaviour
{
    [SerializeField] Logger.LoggerMode _loggerMode;

    private void Start() {
        UpdateLoggerConfiguration();
    }
    private void OnValidate() {
        UpdateLoggerConfiguration();
    }

    private void UpdateLoggerConfiguration(){
        Logger.SetLoggerMode(_loggerMode);
        //Logger.DebugLoggerState();
    }
}
