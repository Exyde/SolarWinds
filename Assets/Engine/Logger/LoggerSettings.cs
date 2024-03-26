using UnityEngine;

namespace ExydeToolbox
{
    [CreateAssetMenu(menuName = "Settings", fileName = "New Logger Settings")]
    public class LoggerSettings : ScriptableObject
    {
        [SerializeField] public Logger.LoggerMode Mode;
        public string InfoColor = "cyan";
        public string EventColor = "purple";
    } 
}
