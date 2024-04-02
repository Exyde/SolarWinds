using System;
using Engine;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private Observer<float> _speed = new Observer<float>(2);
    [SerializeField] Transform _body;

    public Observer<Vector3> _move = new Observer<Vector3>(new Vector3(0, 0, 0));
    
    private void Start()
    {
        _speed.Invoke();
    }

    void FixedUpdate()
    {
        float multiplier = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 2f;

        _move.Value = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _body.position += _move.Value * Time.deltaTime * _speed.Value * multiplier;
    }
}