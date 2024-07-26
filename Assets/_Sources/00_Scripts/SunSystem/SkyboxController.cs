using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ExydeToolbox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Sequence = DG.Tweening.Sequence;

[System.Serializable]
public class SkyboxController
{
    private Sequence transitionSequence;
    
    [SerializeField] public List<SkyboxSettings> _sunStateSettings;
#region Shader Properties
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
#endregion


void Start()
{
    RenderSettings.ambientMode = AmbientMode.Skybox;
    
}

    public void LerpSkyboxSettings(SkyboxSettings to, float duration = 0)
    {
        transitionSequence?.Kill();

        transitionSequence = DOTween.Sequence();
        
        Material skybox = RenderSettings.skybox;
            // skybox.SetTexture(CloudTexture, to._cloudTexture);
    }

    public void LerpShaderColor(Sequence sequence, Material material, int propertyID, Color color, float duration = 0)
    {
        sequence.Insert(0, DOTween.To (() => material.GetColor(propertyID), x => material.SetColor(propertyID, x), color,
            duration));
    }
    
    public void LerpShaderFloat(Sequence sequence, Material material, int propertyID, float value, float duration = 0)
    {
        sequence.Insert(0, DOTween.To (() => material.GetFloat(propertyID), x => material.SetFloat(propertyID, x), value,
            duration));
    }
    
    public void LerpSkyboxSettings(SkyboxSettings a, SkyboxSettings b, float duration)
    {
        transitionSequence?.Kill();
        transitionSequence = DOTween.Sequence();
        
        Material skybox = RenderSettings.skybox;
        //skybox.SetTexture(CloudTexture, b.Material.GetTexture("CloudTexture"));
        
        //Sky
        LerpShaderColor(transitionSequence, skybox, SkyColor, b.Material.GetColor(SkyColor), duration);
        LerpShaderColor(transitionSequence, skybox, HorizonColor, b.Material.GetColor(HorizonColor), duration);
        
        //Stars
        LerpShaderFloat(transitionSequence, skybox, StarDensity, b.Material.GetFloat(StarDensity), 0.2f);
        
        //Cloud
        LerpShaderColor(transitionSequence, skybox, CloudColor, b.Material.GetColor(CloudColor), duration);
        LerpShaderFloat(transitionSequence, skybox, CloudSpeed, b.Material.GetFloat(CloudSpeed), duration);
        LerpShaderFloat(transitionSequence, skybox, CloudHeight, b.Material.GetFloat(CloudHeight), duration);
        LerpShaderFloat(transitionSequence, skybox, CloudEdge, b.Material.GetFloat(CloudEdge), duration);

        //Astral
        LerpShaderColor(transitionSequence, skybox, SunColor, b.Material.GetColor(SunColor), duration);
        LerpShaderFloat(transitionSequence, skybox, SunSize, b.Material.GetFloat(SunSize), duration);
        LerpShaderFloat(transitionSequence, skybox, SunMaskSize, b.Material.GetFloat(SunMaskSize), duration);
        
        transitionSequence.Insert(0, DOTween.To (() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, b.Material.GetColor(SkyColor),
            duration));
        
//        transitionSequence.AppendCallback(() => RenderSettings.skybox = skybox);
        transitionSequence.Play();
    }
    
    public void SetSunState(SunState currentSunState, SunState newSunState, float transitionDuration)
    {
        SkyboxSettings currentSettings = _sunStateSettings.First(x => x.SunState == currentSunState);
        SkyboxSettings newSettings = _sunStateSettings.First(x => x.SunState == newSunState);
        
        Debug.Log($"CurrentSettings : {currentSettings} - newSettings : {newSettings}");

       LerpSkyboxSettings(currentSettings, newSettings, transitionDuration);
        //-- This seems shitty.
//        RenderSettings.skybox.Lerp(currentSettings._skyboxSettings.Material, newSettings._skyboxSettings.Material, transitionDuration);
        //RenderSettings.skybox = newSettings.Material;
    }
}
