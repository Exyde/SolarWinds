using System.Collections;
using UnityEngine;
using Core.GameEvents;
using ExydeToolbox;
using EventType = Core.GameEvents.EventType;

public class MeteoManager : MonoBehaviour, IGameEventManager {
    public SkyboxLerper _skyboxLerper;
    private void OnGameEvent_MeteoManager(EventType eventType, string senderName){

        Engine.Core.Instance.Logger.LogEvent(eventType, senderName, this.GetType().Name);

        HandleCollisionEvents(eventType, senderName);
        HandleRaycastEvents(eventType, senderName);
        HandleTriggerEvents(eventType, senderName);
        HandleSpecialCases(eventType, senderName);
    }
    
    private void Start() {
        // LerpSkyboxAllNight(); //@TODO : Lerp Skybox during all night duration, start day event, etc
    }

    private void OnEnable() { }

    private void OnDisable() { }

    private void RequestRain(){ }

    public void HandleTriggerEvents(EventType eventType, string senderName)
    {        
        if (eventType == EventType.TRIGGER_ENTER){
            RequestRain();
        }
        else if (eventType ==  EventType.TRIGGER_EXIT){
            Engine.Core.Instance.Logger.LogInfo("Setting Sun !");
        }
    }

    public void HandleCollisionEvents(EventType eventType, string senderName) { }

    public void HandleRaycastEvents(EventType eventType, string senderName) { }

    public void HandleSpecialCases(EventType eventType, string senderName){
        if (senderName == "Cube_OnTriggerExit"){
            Engine.Core.Instance.Logger.LogInfo("Get ready for the storm");
        }
    }

    //Skybox Lerp - Need SkyboxConfig ?
    [ContextMenu("Lerp Skybox A->B")]
    public void LerpSkyboxSettingsContextMenuAB(){
        StartCoroutine(_skyboxLerper.LerpSkyboxSettings(_skyboxLerper._skyboxSettingsA, _skyboxLerper._skyboxSettingsB,  _skyboxLerper._timeToLerp));
    }

    [ContextMenu("Lerp Skybox B->A")]
    public void LerpSkyboxSettingsContextMenuBA(){
        StartCoroutine(_skyboxLerper.LerpSkyboxSettings(_skyboxLerper._skyboxSettingsB, _skyboxLerper._skyboxSettingsA, _skyboxLerper._timeToLerp));
    }

    [ContextMenu("Set Skybox Settings : A")]
    public void SetSkyboxSettings(){
        StartCoroutine(_skyboxLerper.LerpSkyboxSettings(_skyboxLerper._skyboxSettingsA, _skyboxLerper._skyboxSettingsA, 0.1f));
    }

    private void LerpSkyboxAllNight(){
        StartCoroutine(_skyboxLerper.LerpSkyboxSettings(_skyboxLerper._skyboxSettingsA, _skyboxLerper._skyboxSettingsA, 60f));
    }
}

[System.Serializable]
public class SkyboxLerper{
    [SerializeField][Range(.5f, 30)] public float _timeToLerp;
    [SerializeField] public SkyboxSettings _skyboxSettingsA;
    [SerializeField] public SkyboxSettings _skyboxSettingsB;

    public IEnumerator LerpSkyboxSettings(SkyboxSettings A, SkyboxSettings B, float timeToLerp){

        float elapsedTime = 0;

        Material skybox = RenderSettings.skybox;
        skybox.SetTexture("_Cloud_Texture", B._cloudTexture);
        
        while (elapsedTime < timeToLerp){
            
            float t = elapsedTime / timeToLerp;
            //skybox.Lerp(matA, matB, t);
            
            //Sky
            skybox.SetColor("_SkyColor", Color.Lerp(A._skyColor, B._skyColor, t));
            skybox.SetColor("_HorizonColor", Color.Lerp(A._horizonColor, B._horizonColor, t));

            //Stars
            skybox.SetFloat("_Star_Density", Mathf.Lerp(A._starsDensity, B._starsDensity, t));

            //Cloud
            skybox.SetColor("_Cloud_Color", Color.Lerp(A._cloudColor, B._cloudColor, t));
            skybox.SetFloat("_Cloud_Speed", Mathf.Lerp(A._cloudSpeed, B._cloudSpeed, t));
            skybox.SetFloat("_Cloud_Height", Mathf.Lerp(A._cloudHeight, B._cloudHeight, t));
            skybox.SetFloat("_Cloud_Edge", Mathf.Lerp(A._cloudEdge, B._cloudEdge, t));

            //Astral
            skybox.SetColor("_Sun_Color", Color.Lerp(A._sunColor, B._sunColor, t));
            skybox.SetFloat("_SunSize", Mathf.Lerp(A._sunSize, B._sunSize, t));
            skybox.SetFloat("_Sun_Mask_Size", Mathf.Lerp(A._sunMaskSize, B._sunMaskSize, t));
            
            elapsedTime += Time.deltaTime;
            //RenderSettings.skybox = skybox;
            yield return null;
        }
    }
}

