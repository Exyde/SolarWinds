using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] TerrainData _terrainData;
    [SerializeField][Tooltip("Number of perlin sampling")][Range(1, 8)] int _octaves; 
    [SerializeField][Tooltip("Modulate the amplitude, a.k.a the distance between each sampling")][Range(0, 1)] float _persistence;
    [SerializeField][Tooltip("Grow frequency between each octaves")][Range(1, 32)] float _lacunarity;
    [SerializeField][Range(0.0001f, 128f)] float _mapScale;
    [SerializeField] Vector3 _terrainSize;

    [SerializeField] float _heightMultiplier;
    [SerializeField] AnimationCurve _heightCurve;

    [Header("Randomness")]
    [SerializeField] int _seed;
    [SerializeField] Vector2 _offset;

    [SerializeField] public bool _autoUpdate;
    [SerializeField] public bool _useFalloffMap;

    [SerializeField] Material _terrainMaterial;

    Terrain _terrain;
    public void GenerateTerrainData(){
        _terrainData = new TerrainData();
        _terrainData.heightmapResolution = 513;
        _terrainData.size = _terrainSize;
        _terrainData.name = "Terrain Data";
        _terrainData.SetHeights(0, 0, GenerateHeights());

        _terrain = Terrain.activeTerrain;

        _terrain.terrainData  = _terrainData;
        _terrain.GetComponent<TerrainCollider>().terrainData = _terrainData;
        _terrain.materialTemplate = _terrainMaterial;
        _terrain.Flush();

        transform.position = new Vector3(-_terrainSize.x/2, 0, -_terrainSize.z/2);

    }

    float[,] GenerateHeights(){
        float[,] map = Noise.GenerateNoiseMap(_terrainData.heightmapResolution, _terrainData.heightmapResolution,
        _seed, _mapScale, _octaves, _persistence, _lacunarity, _offset);


        if (_useFalloffMap){
            float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(_terrainData.heightmapResolution);

            for (int x =0; x < _terrainData.heightmapResolution; x++){
                for (int y =0; y < _terrainData.heightmapResolution; y++){
                    map[x,y] = Mathf.Clamp01(map[x,y] - falloffMap[x,y]);

                    if (x > _terrainData.heightmapResolution * .25f && x < _terrainData.heightmapResolution * .75f && y >  _terrainData.heightmapResolution * .25f && y < _terrainData.heightmapResolution * .75f){
                        map[x,y] = 0;
                    }

                }
            }
        }

        return map;
    }
}
