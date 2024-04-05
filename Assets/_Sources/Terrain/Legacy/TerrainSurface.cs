using UnityEngine;

public class TerrainSurface : MonoBehaviour
{
    #region Global Members
    [Header("Settings")]
    [SerializeField] Vector3 _terrainSize;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField][Tooltip("Number of perlin sampling")][Range(1, 8)] int _octaves; 
    [SerializeField][Tooltip("Modulate the amplitude, a.k.a the distance between each sampling")][Range(0, 1)] float _persistence;
    [SerializeField][Tooltip("Grow frequency between each octaves")][Range(1, 32)] float _lacunarity;
    [SerializeField][Range(0.0001f, 128f)] float _mapScale;
    [SerializeField, HideInInspector] TerrainData _terrainData;
    
    [Header("Randomness")]
    [SerializeField] int _seed;
    [SerializeField] Vector2 _offset;

    [SerializeField] public bool _autoUpdate;
    [SerializeField] public bool _autoUpdateForest;
    [SerializeField] public bool _useFalloffMap;

    [SerializeField] Material _terrainMaterial;
    private Terrain _terrain;
    
    #endregion

    #region Properties
    public TerrainData Data => _terrainData;
    public float HalfX => _terrainData.size.x / 2;
    public float HalfZ => _terrainData.size.z / 2;
    #endregion

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
                }
            }
        }

        return map;
    }
    
    public Vector3 GetRandomTerrainPositionAtHeight(){

        var x = HalfX;
        var y = _terrainData.size.y + 100f;
        var z = HalfZ;

        Vector3 randomPosition = new Vector3(Random.Range(- x, x ), y, Random.Range(- z, z ));
        return randomPosition;
    }
    
}
