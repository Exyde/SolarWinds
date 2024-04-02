using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayer;
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
    [SerializeField] public bool _autoUpdateForest;
    [SerializeField] public bool _useFalloffMap;

    [SerializeField] Material _terrainMaterial;
    Terrain _terrain;

    [Header("Forests")]
    [SerializeField] List<GameObject> _treePrefabs;
    [SerializeField][Range(1, 1000)] int _treeCount;
    [SerializeField] Transform _treeHolder;
    [SerializeField] bool _resetForests;

    [Header ("Fireflies")]
    [SerializeField] GameObject _fireflyPrefab;
    [SerializeField] int _fireflyCount;
    [SerializeField] Transform _fireflyHolder;
    [SerializeField] AnimationCurve _fireflyHeightDistributionCurve;
    
    private void Start(){
        GenerateForest();
        GenerateFireflies();
    }

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

        if (_autoUpdateForest) GenerateForest();

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

    Coroutine _forestRoutine; 
    public void GenerateForest(){
        _forestRoutine = StartCoroutine(CGenerateForest());
    }

    public void GenerateFireflies(){
        SpawnFireflies();
    }
    public void SpawnCreatures(GameObject creaturePrefab, int creatureCount, Transform creatureHolder, AnimationCurve heightCurve = null){
        for (int i = 0; i < creatureCount; i++){
            Vector3 randomPosition = GetRandomTerrainPosition();
            randomPosition += heightCurve == null ? Vector3.up * _fireflyHeightDistributionCurve.Evaluate(Random.value) * _terrainData.size.y : Vector3.zero;
            Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
            GameObject creature = Instantiate(creaturePrefab, randomPosition, Quaternion.Euler(randomRotation));
            creature.transform.parent = creatureHolder;
        }
    }

    void ClearCreatures(Transform creatureHolder){
        creatureHolder.Clear();
    }
    public void SpawnFireflies(){
        ClearCreatures(_fireflyHolder);
        SpawnCreatures(_fireflyPrefab, _fireflyCount, _fireflyHolder, _fireflyHeightDistributionCurve);
    }
    public Vector3 GetRandomTerrainPosition(){
        float halfTerrainX = _terrainData.size.x / 2;
        float halfTerrainZ = _terrainData.size.z / 2;
        float verticalRaycastStart = _terrainData.size.y + 100f;

        Vector3 randomPosition = new Vector3(Random.Range(- halfTerrainX, halfTerrainX), verticalRaycastStart, Random.Range(- halfTerrainZ, halfTerrainZ));
        return randomPosition;
    }

    IEnumerator CGenerateForest(){

        if (_resetForests) ClearForest();



        for (int i = 0; i < _treeCount; i++){
            Vector3 randomPosition = GetRandomTerrainPosition();
            Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);

            //Debug.DrawLine(randomPosition, randomPosition + Vector3.down * verticalRaycastStart * 100f, Color.red, 3f);

            if (Physics.Raycast(randomPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")){
                    --i;
                    continue;
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){

                    GameObject _treePrefab = _treePrefabs[Random.Range(0, _treePrefabs.Count)];
                    randomPosition = hit.point;
                    GameObject tree = Instantiate(_treePrefab, randomPosition, Quaternion.Euler(randomRotation));

                    tree.transform.localScale = Vector3.one * Random.Range(0.5f, 2f);
                    tree.name = $"Tree {i}";
                    tree.transform.parent = _treeHolder;
                }
                

            }
        }

        yield return null;
    }

    public void ClearForest(){
        if(_forestRoutine != null) StopCoroutine(_forestRoutine);
        _treeHolder.Clear();
    }
}
