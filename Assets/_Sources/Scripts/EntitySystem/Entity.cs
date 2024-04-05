using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Core = Engine.Core;

namespace Systems.Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        [Header("Entity Component")]
        [SerializeField] private EntityData _entityData;
        [SerializeField] private float _currentLifetime;
        [SerializeField] private Vibrant.Core.Timer _timer;
        
        private void LogInnerState()
        {
            //Engine.Core.Instance.Logger.Log($"[Entity] : {_entityData.Name} with  [{_currentLifetime} / {_entityData.SpawnLifetime}]");
        }
        
        #region IEntity
        public virtual void Birth()
        {
            Engine.Core.Instance.Logger.Log($"[Entity] : {_entityData.Name} is born !");
            gameObject.name = _entityData.Name;

            _timer = new Vibrant.Core.Timer();
            _timer.RegisterEndEvent(Tick);
        }

        public virtual void Tick()
        {
            _currentLifetime -= _entityData.LossOnTick;
            _currentLifetime = Math.Clamp(_currentLifetime, 0, _entityData.SpawnLifetime);
            LogInnerState();

            if (_currentLifetime <= 0)
            {
                Die();
            }
        }

        protected virtual void SpawnOffspring()
        {
            if (_entityData.OffspringCount == 0) return;

            for (int i = 0; i < _entityData.OffspringCount; i++)
            {
                if (Random.Range(0, 100) > _entityData.OffspringProbability) continue;
                World.Instance.SpawnEntity(_entityData.Offspring, transform.position.RandomPosInCircle(3f));
            }
        }

        public virtual void Die()
        {
            Engine.Core.Instance.Logger.Log(($"[Entity] : {_entityData.Name} is dead !"));

            _timer.Kill();

            SpawnOffspring();

            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
            {
                meshRenderer.material = _entityData.DeathMaterial;
            }
            
            //Disable all behavior and set dead state
           // Destroy(this.gameObject);
        }
        #endregion

        #region Core Behaviour
        private void OnEnable() => World.Instance.Register(this);
        private void OnDisable() => World.Instance.Unregister(this);

        public void Init()
        {
            _currentLifetime = _entityData.SpawnLifetime;
            transform.localScale *= _entityData.SpawnScaleMultiplier;
            
            Birth();
        }

        public void Update()
        {
            _timer.Update();
        }
        #endregion
    }
}