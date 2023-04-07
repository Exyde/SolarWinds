using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLightManager : MonoBehaviour
{
    /// <summary>
    /// Multiplicateur d'intensit� de la lumi�re du soleil
    /// </summary>
    [SerializeField]
    private float _sunLightIntensityMultiplier = .5f;

    /// <summary>
    /// R�f�rence du transform g�rant le soleil et la lune
    /// </summary>
    [SerializeField]
    private Transform _orbitSunAndMoon;

    [SerializeField]
    private Light _sunLight;
    [SerializeField]
    private Light _moonLight;

    /// <summary>
    /// Courbe d'�volution de la lumi�re
    /// </summary>
    [SerializeField]
    private AnimationCurve _curveSunIntensity;

    /// <summary>
    /// Courbe d'�volution de la lumi�re de la lune
    /// </summary>
    [SerializeField]
    private AnimationCurve _curveMoonIntensity;

    private void Start()
    {
        _orbitSunAndMoon.localEulerAngles = new Vector3((0f * 360f) - 90f, 170, 0);
        _sunLight.shadowStrength = 0;
    }

    public void UpdateFromTime(float normalizedTime, bool shadow)
    {
        _orbitSunAndMoon.localEulerAngles = new Vector3((normalizedTime * 360f) - 90f, 170, 0);

        _sunLight.intensity = _curveSunIntensity.Evaluate(normalizedTime) * _sunLightIntensityMultiplier;
        _moonLight.intensity = _curveMoonIntensity.Evaluate(normalizedTime);

        if (shadow)
            UpdateShadowFromTime(normalizedTime);
    }

    public void UpdateShadowFromTime(float normalizedTime)
    {
        UpdateShadow(Mathf.Clamp(_curveSunIntensity.Evaluate(normalizedTime), 0, 1));
    }

    public void UpdateShadow(float shadow)
    {
        _sunLight.shadowStrength = shadow;
    }
}
