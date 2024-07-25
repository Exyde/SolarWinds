using System;
using System.Numerics;
using Engine;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class MimicController : MonoBehaviour
{
    [SerializeField] private Observer<float> _speed = new Observer<float>(2);
    [SerializeField] Transform _body;
    private PlayerController _player;

    [SerializeField] Vector3 _playerPosition;

    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private float _minDistanceToPlayer = 1f;

    [SerializeField] private float _timeToRefreshTargetPos = 2f;
    private float _timeSinceTargetRefresh = 0f;

    public bool forceFollowPlayer;
    
    private void Start()
    {
        _speed.Invoke();

        _player = FindObjectOfType<PlayerController>();
        GetNewTargetPosition(forceFollowPlayer);

    }

    void GetNewTargetPosition(bool forcePlayer)
    {
        _timeSinceTargetRefresh = 0;

        if (forceFollowPlayer)
        {
            _target = _player.transform;
            _targetPos = _player.transform.position;
        }
        else
        {
            var spots = FindObjectsOfType<LandingSpot>();
            _target = spots[Random.Range(0, spots.Length)].transform;

            if (_target == this.transform)
            {
                Debug.LogError("Same target !!");
                int maxIteration = 100;
                int i = 0;
                do
                {
                    _target = spots[Random.Range(0, spots.Length)].transform;
                    i++;
                } while (_target == this.transform && i < maxIteration);
            }
            
            _targetPos = _target.transform.position;
            
        }
        
        //-- Set y to current aniway -- 
        _targetPos.y = transform.position.y;
//        Debug.Log($"${gameObject.name} : got new target -> {_target.name} !", context: gameObject);
    }

    private void Update()
    {
        _playerPosition = _player.transform.position;
        
        _timeSinceTargetRefresh += Time.deltaTime;

        if (_timeSinceTargetRefresh >= _timeToRefreshTargetPos || Vector3.Distance(transform.position, _targetPos) < 0.2f)
        {
            GetNewTargetPosition(forceFollowPlayer);
        }
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }
    
    void MoveTowardsTarget()
    {
        if (forceFollowPlayer && Vector3.Distance(transform.position, _targetPos) < _minDistanceToPlayer) return;
        
        var direction = (_targetPos - transform.position).normalized;            

        _body.position += direction * (Time.deltaTime * _speed.Value);
    }
}