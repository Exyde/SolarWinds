using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    /// <summary>
    /// Instance
    /// </summary>
    public static EnvironmentManager Instance;

    /// <summary>
    /// Lorsque le jour arrive
    /// </summary>
    public event Action OnDay;

    /// <summary>
    /// Lorsque la nuit arrive
    /// </summary>
    public event Action OnNight;

    /// <summary>
    /// Heure de la journ�e
    /// </summary>
    [SerializeField, Range(0, 24)]
    private float _timeOfDay = 24f;

    /// <summary>
    /// Jour en secondes
    /// </summary>
    [SerializeField]
    private float _dayInSeconds = 60f;

    /// <summary>
    /// Gestionnaire de la lumi�re
    /// </summary>
    [SerializeField]
    private EnvironmentLightManager _lightManager;

    /// <summary>
    /// Est-ce que le temps passe ?
    /// </summary>
    [SerializeField] private bool _running;

    /// <summary>
    /// Sommes-nous le jour ou la nuit ?
    /// </summary>
    private bool _day;
    private bool _fixedTime;

    void Start()
    {
        Instance = this;
    }

    public bool IsDay() => _day;
  
    private void Update()
    {
        if (!_running || _fixedTime)
            return;

        UpdateTime();
        UpdateRender();
    }

    public void UpdateTime()
    {
        _timeOfDay += (Time.deltaTime * 24f) / _dayInSeconds;
        _timeOfDay = Mathf.Repeat(_timeOfDay, 24);
    }

    public void UpdateRender(bool shadow = true)
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
