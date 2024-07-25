using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Exploration
{
    public class Orchestrator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _orchestrablesPrefabs;

        private List<IOrchestrable> _orchestrables;

        public float _rate = 1f;
        public float _rotateSpeed = 100f; 
        private void Start()
        {
            RegisterOrchestrables();

            InvokeRepeating(nameof(SpawnOrchestrable), 0, _rate);
        }

        private void RegisterOrchestrables()
        {
            _orchestrables = new List<IOrchestrable>();
            
            foreach (var prefab in _orchestrablesPrefabs)
            {
                if (prefab.TryGetComponent<IOrchestrable>(out var orchestrable))
                {
                    _orchestrables.Add(orchestrable);
                }
            }
        }

        private void Update()
        {
            transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
        }

        private void SpawnOrchestrable()
        {
            var go = Instantiate(GetRandomOrchestrablePrefab(), transform);
            go.transform.position = transform.position.RandomPosInSphere(5);
            go.GetComponent<IOrchestrable>().GetMainSequence().Play();
        }

        IOrchestrable GetRandomOrchestrable() => _orchestrables[Random.Range(0, _orchestrables.Count)];
        GameObject GetRandomOrchestrablePrefab() =>_orchestrablesPrefabs[Random.Range(0,_orchestrablesPrefabs.Count)];

        private void GetRandomPosition()
        {
        }
    }
}