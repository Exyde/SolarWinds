﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExydeToolbox;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkyboxController
{

    [System.Serializable]
    public struct SunStateSettingsPair
    {
        public SunState _sunState;
        public SkyboxSettings _skyboxSettings;
    }
    
    [SerializeField] public List<SunStateSettingsPair> _sunStateSettings;

    private static readonly int CloudTexture = Shader.PropertyToID("_Cloud_Texture");
    private static readonly int SkyColor = Shader.PropertyToID("_SkyColor");
    private static readonly int HorizonColor = Shader.PropertyToID("_HorizonColor");
    private static readonly int StarDensity = Shader.PropertyToID("_Star_Density");
    private static readonly int CloudColor = Shader.PropertyToID("_Cloud_Color");
    private static readonly int CloudSpeed = Shader.PropertyToID("_Cloud_Speed");
    private static readonly int CloudHeight = Shader.PropertyToID("_Cloud_Height");
    private static readonly int CloudEdge = Shader.PropertyToID("_Cloud_Edge");
    private static readonly int SunColor = Shader.PropertyToID("_Sun_Color");
    private static readonly int SunSize = Shader.PropertyToID("_SunSize");
    private static readonly int SunMaskSize = Shader.PropertyToID("_Sun_Mask_Size");


    //Todo : doTween version ? -- Lerp Directly from Material in Settings instead of members 
    public IEnumerator LerpSkyboxSettings(SkyboxSettings A, SkyboxSettings B, float timeToLerp)
    {

        float elapsedTime = 0;

        Material skybox = RenderSettings.skybox;
        skybox.SetTexture(CloudTexture, B._cloudTexture);

        while (elapsedTime < timeToLerp)
        {

            float t = elapsedTime / timeToLerp;
            //skybox.Lerp(matA, matB, t);

            //Sky
            skybox.SetColor(SkyColor, Color.Lerp(A._skyColor, B._skyColor, t));
            skybox.SetColor(HorizonColor, Color.Lerp(A._horizonColor, B._horizonColor, t));

            //Stars
            skybox.SetFloat(StarDensity, Mathf.Lerp(A._starsDensity, B._starsDensity, t));

            //Cloud
            skybox.SetColor(CloudColor, Color.Lerp(A._cloudColor, B._cloudColor, t));
            skybox.SetFloat(CloudSpeed, Mathf.Lerp(A._cloudSpeed, B._cloudSpeed, t));
            skybox.SetFloat(CloudHeight, Mathf.Lerp(A._cloudHeight, B._cloudHeight, t));
            skybox.SetFloat(CloudEdge, Mathf.Lerp(A._cloudEdge, B._cloudEdge, t));

            //Astral
            skybox.SetColor(SunColor, Color.Lerp(A._sunColor, B._sunColor, t));
            skybox.SetFloat(SunSize, Mathf.Lerp(A._sunSize, B._sunSize, t));
            skybox.SetFloat(SunMaskSize, Mathf.Lerp(A._sunMaskSize, B._sunMaskSize, t));

            elapsedTime += Time.deltaTime;
            //RenderSettings.skybox = skybox;
            yield return null;
        }
    }

    public void SetSunState(SunState currentSunState, SunState newSunState, float transitionDuration)
    {
        SunStateSettingsPair currentSettings = _sunStateSettings.First(x => x._sunState == currentSunState);
        SunStateSettingsPair newSettings = _sunStateSettings.First(x => x._sunState == newSunState);
        
        Debug.Log($"CurrentSettings : {currentSettings._skyboxSettings} - newSettings : {newSettings._skyboxSettings}");

       
        //-- This seems shitty.
//        RenderSettings.skybox.Lerp(currentSettings._skyboxSettings.Material, newSettings._skyboxSettings.Material, transitionDuration);
        RenderSettings.skybox = newSettings._skyboxSettings.Material;
    }
}
