using System;
using DG.Tweening;
using UnityEngine;

public enum SunState { Sunborn, Sunset, Sunrise, Sunburn, Sundeath, Eclipse}

public class SunStateManager : MonoBehaviour
{
    [SerializeField] SunState _sunState = SunState.Sunborn;
    [SerializeField] SkyboxController _skyboxController = new SkyboxController();

    public static Action<SunState> OnSunStateChanged;

    public float swapTimeBetweenStates = 4f;
    public float transitionDuration = 10F;

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
        Sequence swapSequence = DOTween.Sequence();

        swapSequence.InsertCallback(0, () => SetSunState(SunState.Sunborn));
        swapSequence.AppendInterval(swapTimeBetweenStates + transitionDuration / 4f);
        swapSequence.AppendCallback( () => SetSunState(SunState.Sunset));
        swapSequence.AppendInterval(swapTimeBetweenStates);
        
        swapSequence.SetLoops(-1);
        swapSequence.Play();

    }

    public void SetSunState(SunState newSunState)
    {
        _skyboxController.SetSunState(_sunState, newSunState, transitionDuration);
        SunState = newSunState;
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
    
    [ContextMenu("Lerp Skybox A->B")]
    public void LerpSkyboxSettingsContextMenuAB(){
    }

    [ContextMenu("Lerp Skybox B->A")]
    public void LerpSkyboxSettingsContextMenuBA(){
    }

    [ContextMenu("Set Skybox Settings : A")]
    public void SetSkyboxSettings(){
    }
}
