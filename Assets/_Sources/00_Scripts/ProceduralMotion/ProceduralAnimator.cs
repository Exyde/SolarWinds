using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    [SerializeField] private Transform[] _limbTransformTargets;
    [SerializeField] private float _stepSize = 1f;

    [Header("Ground Feet Settings")]
    [SerializeField] LayerMask _groundLayerMask = default;
    [SerializeField] private float _raycastRange = 2f;

    private int _limbsCount;
    private ProceduralLimb[] _proceduralLimbs;

    //Handling motion
    private Vector3 _lastBodyPos;
    private Vector3 _velocity;

    void Start()
    {
        SetupLimbs();
    }

    private void SetupLimbs(){
        _limbsCount = _limbTransformTargets.Length;
        _proceduralLimbs = new ProceduralLimb[_limbsCount];
        Transform limbTransform;

        for (int i = 0; i < _limbsCount; ++i)
        {
            limbTransform = _limbTransformTargets[i];
            _proceduralLimbs[i] = new ProceduralLimb()
            {
                _IKTarget = limbTransform,
                _defaultPos = limbTransform.localPosition,
                _lastPos = limbTransform.position
            };
        }
    }

    private void FixedUpdate() {
        _velocity = transform.position - _lastBodyPos;
        _lastBodyPos = transform.position;

        Vector3[] desiredPos = new Vector3[_limbsCount];
        float bestDst = _stepSize;
        int limbToMove = 1;

        for (int i = 0; i < _limbsCount; ++i)
        {
            if (_proceduralLimbs[i]._isMoving) continue; // limb already moving: can't move again!

            desiredPos[i] = transform.TransformPoint(_proceduralLimbs[i]._defaultPos);
            float dist = (desiredPos[i] + _velocity - _proceduralLimbs[i]._lastPos).magnitude;
            if (dist > bestDst)
            {
                bestDst = dist;
                limbToMove = i;
            }
        }

        for (int i = 0; i < _limbsCount; ++i)
            if (i != limbToMove)
                _proceduralLimbs[i]._IKTarget.position = _proceduralLimbs[i]._lastPos;

        // move the selected leg to its "desired" position
        if (limbToMove != -1)
        {
            Vector3 targetPoint = desiredPos[limbToMove];
            targetPoint = RaycastToGround(targetPoint, transform.up);
            _proceduralLimbs[limbToMove]._IKTarget.position = targetPoint;
            _proceduralLimbs[limbToMove]._lastPos = targetPoint;
        }

    }

    private Vector3 RaycastToGround(Vector3 pos, Vector3 up)
    {
        Vector3 point = pos;

        Ray ray = new Ray(pos + _raycastRange * up, -up);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f * _raycastRange, _groundLayerMask))
            point = hit.point;
        return point;
    }

    public class ProceduralLimb{
        public Transform _IKTarget;
        public Vector3 _defaultPos;
        public Vector3 _lastPos;
        public bool _isMoving = false;
    }

}
