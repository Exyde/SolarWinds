using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;
    public event Action OnDay;
    public event Action OnNight;
    
    [SerializeField, Range(0, 24)]
    private float _timeOfDay = 24f;
    
    [SerializeField]
    private float _dayInSeconds = 60f;
    
    [SerializeField]
    private EnvironmentLightManager _lightManager;

    [SerializeField] private bool _running;
    
    private bool _day;
    private bool _fixedTime;

    void Start() => Instance = this;

    public bool IsDay => _day;
  
    private void Update()
    {
        if (!_running || _fixedTime)
            return;

        UpdateTime();
        UpdateRender();
    }

    private void UpdateTime()
    {
        _timeOfDay += (Time.deltaTime * 24f) / _dayInSeconds;
        _timeOfDay = Mathf.Repeat(_timeOfDay, 24);
    }

    private void UpdateRender(bool shadow = true)
    {
        _lightManager.UpdateFromTime(_timeOfDay / 24f, shadow);
        UpdateDayNightCycle();
    }

    private void UpdateDayNightCycle()
    {
        if (_day)
        {
            if (_timeOfDay > 18 || _timeOfDay < 6)
            {
                _day = false;
                if (_running) OnNight?.Invoke();
            }
        }
        else
        {
            if (_timeOfDay < 18 && _timeOfDay > 6)
            {
                _day = true;
                if (_running) OnDay?.Invoke();
            }
        }
    }

    public void SetManualTime(float time)
    {
        _timeOfDay = time;
        UpdateRender(false);
    }

    public void SetShadow(float shadow) =>  _lightManager.UpdateShadow(shadow);
    public void StartCycling() => _running = true;
}
