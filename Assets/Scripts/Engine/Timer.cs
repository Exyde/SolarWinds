using UnityEngine;
using System;

[System.Serializable]
public class Timer
{
    [Header("Timer Settings")]
    [SerializeField] string _timerName;
    [SerializeField] float _timerDuration;
    [SerializeField] bool _timerEnabled;
    [SerializeField] bool _loop = true;

    [Header("Runtime Value")]
    [SerializeField] float _timeElapsed = 0;

    [SerializeField] public Action OnTimerEnd;

    public void Update()
    {
        if (!_timerEnabled ) return;

       _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= _timerDuration){
            OnTimerEnd?.Invoke();
            if (_loop)_timeElapsed = 0;
            else ToggleTimer(false);
        }
    }

    public void SetTimerDuration(float _durationInSeconds) => _timerDuration = _durationInSeconds;
    public void ResetTimerValue() => _timeElapsed = 0;
    public float GetTimerDuration() => _timerDuration;
    public int GetTimeElapsedInt() => (int)_timeElapsed;
    public float GetTimeElapsed() => _timeElapsed;
    public void ToggleTimer(bool state) => _timerEnabled = state;
    public void PauseTimer() => ToggleTimer(false);
    public void PlayTimer() => ToggleTimer(true);

}