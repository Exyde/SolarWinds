using UnityEngine;

namespace ExydeToolbox
{
    [CreateAssetMenu(menuName = "Settings/Skybox Settings", fileName = "New Skybox Settings")]
    public class SkyboxSettings : ScriptableObject
    {
        [SerializeField] private SunState _sunState;
        [SerializeField] private Material _skyboxMaterial;

        public SunState SunState => _sunState;
        public Material Material => _skyboxMaterial;
        
        /*
        [Header ("Sky")]
        [SerializeField] public Color _skyColor;
        [SerializeField] public Color _horizonColor;

        [Header ("Stars")]
        [SerializeField][Range(0, 1000)] public float _starsDensity;

        [Header ("Clouds")]
        [SerializeField] public Texture _cloudTexture;
        [SerializeField] public Color _cloudColor;
        [SerializeField][Range(0, 1)] public float _cloudSpeed;
        [SerializeField][Range(0, 1)] public float _cloudHeight;
        [SerializeField][Range(0, 1)] public float _cloudEdge;

        [Header ("Astral Plane")]
        [ColorUsageAttribute(true,true)]
        [SerializeField] public Color _sunColor;
        [SerializeField][Range(0, 2)] public float _sunSize;
        [SerializeField][Range(0, 1)] public float _sunMaskSize;
*/
    }
}