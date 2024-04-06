using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Entities
{
    [Serializable]
    public class FollowBehavior : EntityBehavior
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float timeBetweenTargetUpdate = 1f;

        [SerializeField] private EntityData _targetType;

        Vector3 targetPos;

        public override void Setup()
        {
            //_target = FindObjectOfType<PlayerController>().gameObject;
            //InvokeRepeating("UpdateTargetPos", 0f, timeBetweenTargetUpdate);

            _speed += Random.Range(-1f, 2f);
            timeBetweenTargetUpdate += Random.Range(.1f, 6f);
        }

        public override void Process()
        {
            //transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime); 
        }
        
        void UpdateTargetPos(){
            targetPos = _target.transform.position;
            //transform.LookAt(targetPos);
        }
    }
}