using System.Collections.Generic;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random =  UnityEngine.Random;

namespace Systems.Entities
{
    [CreateAssetMenu(menuName = "Vibrant/Entity", fileName = "ED_New Entity")]
    public class EntityData : ScriptableObject
    {
        [Header("Spawning")]
        [SerializeField] GameObject _prefab;
        [SerializeField] private string _name;
        [SerializeField] private bool _randomizeSpawnPos;
        [SerializeField] private Vector3 randomSpawnRandomness;
        
        [Header("Lifetime")]
        [SerializeField] private float _maxLifetime;
        [SerializeField] private float _lossOnTick;
        [SerializeField] private bool _randomizeLifetime;
        [SerializeField] private Vector2 _randLifetime = new Vector2();

        [Header("Scale")] [SerializeField] private Vector2 _scaleMinMax;
        [SerializeField] private bool _randomizeScale;
        [SerializeField] private AnimationCurve _scaleOverLife;
        
        [Header("Offsprings")]
        [SerializeField] private EntityData _offspring;
        [SerializeField, UnityEngine.Range(0, 8)] private int _offspringCount;
        [SerializeField, UnityEngine.Range(0, 100)] private float _offspringProbability;

        [Header("Death")]
        [SerializeField] private Material _deathMaterial;

        [Header("Behaviors")] [SerializeField] private List<PolymorphValue<EntityBehavior>> _behaviors;
        public List<PolymorphValue<EntityBehavior>> Behaviours => _behaviors;
        
        #region Getters
        public string Name => _name;
        public GameObject Prefab => _prefab;
        public float SpawnLifetime => _randomizeLifetime ? Random.Range(_randLifetime.x, _randLifetime.y) : _maxLifetime;
        public float LossOnTick => _lossOnTick;
        public EntityData Offspring => _offspring;
        public int OffspringCount => _offspringCount;
        public float OffspringProbability => _offspringProbability;
        public Material DeathMaterial => _deathMaterial;

        public bool RandomizeSpawnPos => _randomizeSpawnPos;
        public Vector3 RandomSpawnOffset => randomSpawnRandomness;
        
        //Scale
        public float SpawnScaleMultiplier => _randomizeScale ? Random.Range(_scaleMinMax.x, _scaleMinMax.y): 1;

        #endregion
        
    }
}