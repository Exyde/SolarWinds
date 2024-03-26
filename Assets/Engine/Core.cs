using DG.Tweening;
using UnityEngine;
using ExydeToolbox;
using Logger = ExydeToolbox.Logger;

namespace Engine
{
    [DefaultExecutionOrder(-100)]
    public class Core : MonoBehaviour
    {
        public static Core Instance;
        public Logger Logger => _logger;
        [SerializeField] private Logger _logger;

        [Header("Settings")]
        [SerializeField] private LoggerSettings _loggerSettings;
        
        private void Awake()
        {
            Instance = this;
            InitializeCoreSystems();
        }

        void InitializeCoreSystems()
        {
            _logger = new Logger(_loggerSettings);

            DOTween.Init();
        }
    }
}