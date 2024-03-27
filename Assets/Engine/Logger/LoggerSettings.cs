using UnityEngine;

namespace ExydeToolbox
{
    [CreateAssetMenu(menuName = "Settings/Logger Settings", fileName = "New Logger Settings")]
    public class LoggerSettings : ScriptableObject
    {
        [SerializeField] public Logger.LoggerMode Mode;
        [SerializeField] private Color _infoColor;
        [SerializeField] private Color _eventColor;

        public Color EventColor => _eventColor;
        public Color InfoColor => _infoColor;
    } 
}
