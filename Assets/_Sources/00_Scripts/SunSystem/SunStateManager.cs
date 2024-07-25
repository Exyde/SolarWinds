using System;
using UnityEngine;

public enum SunState { Sunborn, Sunset, Sunrise, Sunburn, Sundeath, Eclipse}

public class SunStateManager : MonoBehaviour
{
    [SerializeField] SunState _sunState = SunState.Sunborn;
    [SerializeField] SkyboxController _skyboxController = new SkyboxController();

    public static Action<SunState> OnSunStateChanged;

    public SunState SunState
    { 
        get => _sunState;
        set
        {
            _sunState = value;
            OnSunStateChanged?.Invoke(_sunState);
        }
    }

    private void Start()
    {
        SunState = SunState.Sunborn;
    }

    private void OnEnable()
    {
        OnSunStateChanged += Log;
    }

    private void OnDisable()
    {
        OnSunStateChanged -= Log;
    }

    private void Log(SunState newSunState)
    {
        Debug.Log($"[SunStateManager] : current SunState is {newSunState}");
    }
    
    //Skybox Lerp - Need SkyboxConfig ?
    [ContextMenu("Lerp Skybox A->B")]
    public void LerpSkyboxSettingsContextMenuAB(){
        StartCoroutine(_skyboxController.LerpSkyboxSettings(_skyboxController._skyboxSettingsA, _skyboxController._skyboxSettingsB,  _skyboxController._timeToLerp));
    }

    [ContextMenu("Lerp Skybox B->A")]
    public void LerpSkyboxSettingsContextMenuBA(){
        StartCoroutine(_skyboxController.LerpSkyboxSettings(_skyboxController._skyboxSettingsB, _skyboxController._skyboxSettingsA, _skyboxController._timeToLerp));
    }

    [ContextMenu("Set Skybox Settings : A")]
    public void SetSkyboxSettings(){
        StartCoroutine(_skyboxController.LerpSkyboxSettings(_skyboxController._skyboxSettingsA, _skyboxController._skyboxSettingsA, 0.1f));
    }

    private void LerpSkyboxAllNight(){
        StartCoroutine(_skyboxController.LerpSkyboxSettings(_skyboxController._skyboxSettingsA, _skyboxController._skyboxSettingsA, 60f));
    }
}
