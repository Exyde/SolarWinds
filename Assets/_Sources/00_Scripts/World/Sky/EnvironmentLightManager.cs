using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLightManager : MonoBehaviour
{
    [SerializeField] private float _sunLightIntensityMultiplier = .5f;
    [SerializeField] private Transform _orbitSunAndMoon;
    [SerializeField] private Light _sunLight;
    [SerializeField] private Light _moonLight;
    
    [SerializeField]
    private AnimationCurve _curveSunIntensity;
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

    private void UpdateShadowFromTime(float normalizedTime)
    {
        UpdateShadow(Mathf.Clamp(_curveSunIntensity.Evaluate(normalizedTime), 0, 1));
    }

    public void UpdateShadow(float shadow)
    {
        _sunLight.shadowStrength = shadow;
    }
}
