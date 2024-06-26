using DG.Tweening;
using Exploration;
using UnityEngine;

public class DrippingSphere : MonoBehaviour, IOrchestrable
{
    [SerializeField] private float _timeToGrow;
    [SerializeField] Vector3 _targetScale = Vector3.one;
    [SerializeField] Ease _easeMode = Ease.Linear;
    [SerializeField] private bool _loop = true;

    [SerializeField] private ParticleSystem _particleSystem;
    public Sequence GetMainSequence()
    {
        _timeToGrow += Random.Range(-0.5f, 0.5f);
        _targetScale = Random.Range(0f, 1f) > 0.5f ? Vector3.one * 1.2f : Vector3.zero;
        
        var seq =  SequenceLibrary.GetScaleSequence(transform, _targetScale, _timeToGrow, _loop, _easeMode);
        seq.InsertCallback(_timeToGrow, () => _particleSystem.Play());
        return seq;
    }
}
