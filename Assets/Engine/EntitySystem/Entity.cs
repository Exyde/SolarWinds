using System;
using Extensions;
using UnityEngine;
using Vibrant;
using Vibrant.Core;
using Logger = Vibrant.Core.Logger;
using Random = UnityEngine.Random;

namespace Entity
{
    public class Entity : MonoBehaviour, IEntity
    {
        [SerializeField] private EntityData _entityData;
        
        [Header("Debug")]
        [SerializeField] private float _currentLifetime;
        
        //Timer
        [SerializeField] private Timer _timer;
        
        private void Log()
        {
            Logger.Instance.LogMessage($"[Entity] : {_entityData.Name} with  [{_currentLifetime} / {_entityData.SpawnLifetime}]");
        }
        
        #region IEntity
        public virtual void Birth()
        {
            Logger.Instance.LogMessage($"[Entity] : {_entityData.Name} is born !");   
            gameObject.name = _entityData.Name + $" : {Time.time}";

            _timer = new Timer();
            _timer.RegisterEndEvent(Tick);
        }

        public virtual void Tick()
        {
            _currentLifetime -= _entityData.LossOnTick;
            _currentLifetime = Math.Clamp(_currentLifetime, 0, _entityData.SpawnLifetime);
            Log();

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
            Logger.Instance.LogMessage(($"[Entity] : {_entityData.Name} is dead !"));

            _timer.Kill();

            SpawnOffspring();

            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
            {
                meshRenderer.material = _entityData.DeathMaterial;
            }
            
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