using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Engine.CitationPlugin.GenericMessagePlugin;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FishFaller : MonoBehaviour
{
    [SerializeField] private List<GameObject> _fishPrefabs;

    [FormerlySerializedAs("_spawnDelay")] [SerializeField] private float _initialSpawnInterval = 0.2f;
    [FormerlySerializedAs("_spawnDelay")] [SerializeField] private float _minSpawnInterval = 0.01f;

    public int maxFishes = 5000;
    public float bounds = 50;
    private Sequence _sequence;

    private HashSet<GameObject> _allFishes = new HashSet<GameObject>();

    [SerializeField] private int _fishInstances = 0;
    private static readonly int ColorProperty = Shader.PropertyToID("_BaseColor");

    public GenericMessageDisplayer _messageDisplayer;

    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    void Start()
    {
        _sequence = DOTween.Sequence();

        _sequence.AppendInterval(_initialSpawnInterval);
        _sequence.AppendCallback(() =>
        {
            var prefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Count)];
            var randomPos = new Vector3(Random.Range(-bounds, bounds), transform.position.y, Random.Range(-bounds, bounds));
            _allFishes.Add(Instantiate(prefab, randomPos, Quaternion.identity, transform));

            _fishInstances++;
            if (_initialSpawnInterval > _minSpawnInterval) _initialSpawnInterval -= 0.01f;
            
            if (_fishInstances == maxFishes)
            {
                ChangeAllFishesMaterial();
                _messageDisplayer.ShowMessage(new GenericMessageDisplayer.Message
                {
                    Content = "Many fishes died...",
                    SubContent = $"{timer} since you entered this room...",
                    InDuration = 1,
                    DisplayDuration = 8,
                    OutDuration = 1,
                    AutoSkip = false
                });
                _sequence.SetLoops(0);
                _sequence.Kill();
            }
        });

        _sequence.SetLoops(-1);
        _sequence.Play();
    }

    void ChangeAllFishesMaterial()
    {
        //Cleaner and perf if they all use the same mat but still...

        foreach (var fish in _allFishes)
        {
            fish.GetComponent<MeshRenderer>().material.SetColor(ColorProperty, Color.red);
        }
    }


}
