using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vibrant.Core;

using Random = UnityEngine.Random;

namespace Systems.Entities
{
    public class World : SingletonManager<World, IEntity>, ISerializationCallbackReceiver
    {
        #region Global Members
        
        public static readonly float TickTime = 1F;
        [Header("Forests")]
        [SerializeField] List<GameObject> _treePrefabs;
        [SerializeField][Range(1, 1000)] int _treeCount;
        [SerializeField] Transform _treeHolder;

        [Header ("Fireflies")]
        [SerializeField] GameObject _fireflyPrefab;
        [SerializeField] int _fireflyCount;
        [SerializeField] Transform _fireflyHolder;
        [SerializeField] AnimationCurve _fireflyHeightDistributionCurve;

        [SerializeField] private TerrainSurface _terrain;
        
        [SerializeField] private List<EntityData> _entityDatas;

        [Header("Debug")]
        [SerializeField] private List<Entity> _debugRegisteredEntities = new();
        
        private Coroutine _forestRoutine;
        #endregion

        
        #region Core Behavior
        private void OnEnable()
        {
            _entityDatas.Clear();
            var array = Resources.LoadAll("Entity");
            _entityDatas = array.Select(x => x as EntityData).ToList();
        }

        private void Start()
        {
            _forestRoutine = StartCoroutine(CGenerateForest());

            //Check
            _fireflyHolder.Clear();
            SpawnCreatures(_entityDatas[0], _fireflyCount, _fireflyHolder, _fireflyHeightDistributionCurve);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               // SpawnRandomEntity();
            }
        }
        #endregion

        #region Spawn
        public void SpawnEntity(EntityData data, Vector3 position, Transform parent = null)
        {
            Vector3 spawnPos = data.RandomizeSpawnPos ? position.RandomPosInSphere(data.RandomSpawnOffset) : position;
            
            GameObject go = Instantiate(data.Prefab, spawnPos, Quaternion.identity, parent ? parent : transform);
            Entity entity = go.GetComponent<Entity>();
            entity.Init();
        }
        void SpawnRandomEntity()
        {
            int randIndex = Random.Range(0, _entityDatas.Count);
            EntityData entity = _entityDatas[randIndex];

            SpawnEntity(entity, Vector3.zero.RandomPosInCircle(20));
        }
        
 
        private void SpawnCreatures(EntityData entity, int creatureCount, Transform creatureHolder, AnimationCurve heightCurve = null){
            for (int i = 0; i < creatureCount; i++){
                Vector3 randomPosition = _terrain.GetRandomTerrainPositionAtHeight();
                randomPosition += heightCurve == null ? Vector3.up * _fireflyHeightDistributionCurve.Evaluate(Random.value) * _terrain.Data.size.y : Vector3.zero;
                //Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                SpawnEntity(entity, randomPosition, creatureHolder);
            }
        }
        #endregion
        
        #region Forest
        IEnumerator CGenerateForest(){
            
            ClearForest();
            
            for (int i = 0; i < _treeCount; i++){
                Vector3 randomPosition = _terrain.GetRandomTerrainPositionAtHeight();
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

        private void ClearForest(){
            if(_forestRoutine != null) StopCoroutine(_forestRoutine);
            _treeHolder.Clear();
        }
        #endregion

        #region Serialization
        public void OnBeforeSerialize()
        {
            _debugRegisteredEntities.Clear();
            
            foreach (var entity in Elements)
            {
                _debugRegisteredEntities.Add(entity as Entity);
            }
        }

        public void OnAfterDeserialize()
        {
            Elements.Clear();
            
            foreach (var entity in _debugRegisteredEntities)
            {
                Elements.Add(entity);
            }
        }
        #endregion
    }
}