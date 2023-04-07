using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEntity : MonoBehaviour
{

    [SerializeField] float _speed = 1.0f;
    [SerializeField] float _speedThresholdOnRandom;
    [SerializeField] Vector3 _direction;
    [SerializeField] bool _randomizeDirection = false;
    [SerializeField] bool _randomizeSpeed = false;

    private void Start() {
       if (_randomizeDirection) _direction = new Vector3(0, GetRandomSign(), 0);
       if (_randomizeSpeed) _speed = Random.Range(- _speedThresholdOnRandom, _speedThresholdOnRandom);
    }
    void Update() => transform.Rotate(_direction * _speed * Time.deltaTime);

    float GetRandomSign() => Mathf.Sign(Random.Range(-1f, 1f));
}
