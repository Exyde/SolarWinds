using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using Vibrant;
using Vibrant.Core;

using Random = UnityEngine.Random;

namespace Entity
{
    public class World : SingletonManager<World, IEntity>, ISerializationCallbackReceiver
    {
        public static readonly float TickTime = 1F;

        [SerializeField] private List<EntityData> _entityDatas;

        [Header("Debug")]
        [SerializeField] private List<Entity> _debugRegisteredEntities = new();
        
        #region Core Behavior
        private void OnEnable()
        {
            _entityDatas.Clear();
            var array = Resources.LoadAll("Entity");
            _entityDatas = array.Select(x => x as EntityData).ToList();
        }
        
        private void Update()
        {
           
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnRandomEntity();
            }
        }
        #endregion

        #region Spawn
        public void SpawnEntity(EntityData data, Vector3 position)
        {
            Vector3 spawnPos = data.RandomizeSpawnPos ? position.RandomPosInSphere(data.RandomSpawnOffset) : position;
            
            GameObject go = Instantiate(data.Prefab, spawnPos, Quaternion.identity, transform);
            Entity entity = go.GetComponent<Entity>();
            entity.Init();
        }
        void SpawnRandomEntity()
        {
            int randIndex = Random.Range(0, _entityDatas.Count);
            EntityData entity = _entityDatas[randIndex];

            SpawnEntity(entity, Vector3.zero.RandomPosInCircle(20));
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