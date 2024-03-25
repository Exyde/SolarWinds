using UnityEngine;

namespace ExydeToolbox
{
    [CreateAssetMenu(menuName = "Settings", fileName = "New Logger Settings")]
    public class LoggerSettings : ScriptableObject
    {
        [SerializeField] public Logger.LoggerMode Mode;
        //Colors - Add Settingd
        public readonly string InfoColor = "cyan";
        public readonly string EventColor = "purple";
        
    } 
}
