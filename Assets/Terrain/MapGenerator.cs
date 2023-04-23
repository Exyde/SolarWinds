using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public const int MAP_CHUNK_SIZE = 241; //-1, divisible by 2, 4, 6, 8, 10, 12, perfect for LOD, and under the unity mesh size limit (255² = 65025)
    [SerializeField][Range(0, 6)] int _levelOfDetail; //so that with multiply it by to OnValidate to get a sane LOD value !
    public enum DrawMode{
        NoiseMap,
        ColorMap,
        Mesh,
        FalloffMap
    }

    [SerializeField] DrawMode _drawMode = DrawMode.NoiseMap;

    [Header("Settings")]
    [SerializeField][Range(0.0001f, 32f)] float _mapScale;
    [SerializeField][Tooltip("Number of perlin sampling")][Range(1, 8)] int _octaves; 
    [SerializeField][Tooltip("Modulate the amplitude, a.k.a the distance between each sampling")][Range(0, 1)] float _persistence;
    [SerializeField][Tooltip("Grow frequency between each octaves")][Range(1, 32)] float _lacunarity;

    [Header("FalloffSettings - Island Mode")]
    [SerializeField] bool _useFalloffMap = true;
    private float[,] _falloffMap;

    [Header("Mesh Settings")]
    [SerializeField] float _heightMultiplier = 2f;
    [SerializeField] AnimationCurve _meshHeightCurve;

    [Header("Randomness")]
    [SerializeField] int _seed;
    [SerializeField] Vector2 _offset;

    [SerializeField] public bool _autoUpdate;

    [Header("Regions")]
    [SerializeField] TerrainType[] _regions;

    MapDisplay _mapDisplay;


    void Awake() {
        _falloffMap = FalloffGenerator.GenerateFalloffMap(MAP_CHUNK_SIZE);
    }

    public void GenerateTerrain(){
        float [,] noiseMap = Noise.GenerateNoiseMap(MAP_CHUNK_SIZE, MAP_CHUNK_SIZE, _seed,  _mapScale, _octaves, _persistence, _lacunarity, _offset); 

        Color[] colorMap = new Color[MAP_CHUNK_SIZE * MAP_CHUNK_SIZE];

        //Getting Map Coordinates
        for(int y = 0; y < MAP_CHUNK_SIZE; y++){
            for(int x = 0; x < MAP_CHUNK_SIZE; x++){

                if (_useFalloffMap){
                    noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - _falloffMap[x,y]); //Typical shader stuff - Wondering why this is not done in shader ?
                }

                float currentHeight = noiseMap[x, y]; 

                //Getting the correct region color
                for (int i = 0; i < _regions.Length; i++){
                    if (currentHeight <= _regions[i]._heightTreshold){
                        colorMap[y * MAP_CHUNK_SIZE + x] = _regions[i]._color;
                        break; //We break at the first occurence
                    }
                }   
            }
        }

        RequestDisplayMap(noiseMap, colorMap);

    }

    private void RequestDisplayMap(float[,] heightMap, Color[] colorMap){
        _mapDisplay = GetComponent<MapDisplay>();

        if (_drawMode == DrawMode.NoiseMap){
            _mapDisplay.DrawTexture(TextureGenerator.GenerateTextureFromHeightMap(heightMap));
        } else if (_drawMode == DrawMode.ColorMap){
            _mapDisplay.DrawTexture(TextureGenerator.GenerateTextureFromColorMap(colorMap, MAP_CHUNK_SIZE, MAP_CHUNK_SIZE));
        } else if (_drawMode == DrawMode.Mesh){
            _mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, _heightMultiplier, _meshHeightCurve, _levelOfDetail), TextureGenerator.GenerateTextureFromColorMap(colorMap, MAP_CHUNK_SIZE, MAP_CHUNK_SIZE));
        } else if (_drawMode == DrawMode.FalloffMap){
            float[,] map = FalloffGenerator.GenerateFalloffMap(MAP_CHUNK_SIZE);
            _mapDisplay.DrawTexture(TextureGenerator.GenerateTextureFromHeightMap(map));
        } 
    }

    private void OnValidate() {

        //Sanity check but that should never happens.
        if (_lacunarity < 1){
            _lacunarity = 1;
        }

        if (_octaves < 0){
            _octaves = 1;
        }

        _falloffMap = FalloffGenerator.GenerateFalloffMap(MAP_CHUNK_SIZE);
    }
}

[System.Serializable]
public struct TerrainType{
    public string _name;
    [Range(0, 1)] public  float _heightTreshold;
    public Color _color;
}
