using System;
using UnityEngine;
using Systems.Entities;


namespace Vibrant.Core
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _elapsedTime;
        [SerializeField] private bool _enabled;
        [SerializeField] private bool _isOneShot;
        [SerializeField] private float? _tickTime;
        [SerializeField] private Action OnTimerEnd;

        public Timer(float? timeTick = null, bool isOneShot = false, bool enabled = true)
        {
            _tickTime = timeTick ?? World.TickTime;
            _enabled = enabled;
            _isOneShot = isOneShot;
            _elapsedTime = 0;
        }

        public void Start()
        {
            _enabled = true;
            _elapsedTime = 0;
        }

        public void Pause() => _enabled = false;
        public void UnPause() => _enabled = true;

        public bool IsActive => _enabled;
        public bool HasEvent => OnTimerEnd != null;
        
        public void Update()
        {
            if (!_enabled) return;
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _tickTime)
            {
                OnTimerEnd?.Invoke();
                _elapsedTime = 0;

                if (_isOneShot)
                {
                    _enabled = false;
                }
            }
        }

        public void RegisterEndEvent(Action callback) => OnTimerEnd += callback;
        public void UnregisterEndEvent(Action callback) => OnTimerEnd -= callback;
        
        public void Kill()
        {
            _enabled = false;
        }
    }
}