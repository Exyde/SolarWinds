using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLegsController : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform[] _IKFootTargets;
    [SerializeField] Transform[] _bodyAttachedPoints;
    Vector3[] _stepGroundTargetPoints;

    [Header("Ground Check Settings")]
    [SerializeField] Vector3 _verticalGroundOffset = new Vector3(0, 5f, 0);
    [SerializeField][Range(0.2f, 25f)] float _groundCheckDst = .5f;

    [Header("Steps Settings")]
    [SerializeField][Range(0.2f, 2f)] float _stepSize = .5f;
    [SerializeField][Range(1f, 200f)] float _stepSpeed = 40f;
    [SerializeField][Range(.1f, 3f)] float _stepHeightMultiplier = 1f;
    [SerializeField] AnimationCurve _stepCurve;
    [SerializeField] bool _tryUseParabolicMovement;

    [Header("Body position")]
    [SerializeField] Vector3 _bodyOffset = new Vector3(0,.5f, 0);
    [SerializeField][Range(0.2f, 16f)] float _bodyPositionUpdateSpeed = 1f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] bool  _rotateBody;
    [SerializeField][Range(1f, 8f)] float _bodyRotationSpeed = 1f;
    [SerializeField] float _maxBodyRotation = 15f;


    [Header("Gizmos Color")]
    [SerializeField] Color _footDisplayColor = Color.cyan;

    int _footCount;
    bool[] _isMovingFoot;

    void Start()
    {
        _footCount = _IKFootTargets.Length;
        _stepGroundTargetPoints = new Vector3[_footCount];
        _isMovingFoot = new bool[_footCount];

        //Setup Body Target Points
        for (int i = 0; i < _footCount; i++){
            //_bodyTargetPoints[i].position = _IKFootTargets[i].position;
            _stepGroundTargetPoints[i] = _IKFootTargets[i].position;
        }
    }

    void FixedUpdate()
    {
        UpdateFootPositions();
        SnapBodyTargetToGround();
        UpdateStepGroundTargetsPoints();
        UpdateVerticalBodyPosition();
        UpdateBodyRotation();
    }

    void UpdateStepGroundTargetsPoints(){
        for (int i = 0; i < _footCount; i++){
                float distance = Vector3.Distance(_bodyAttachedPoints[i].position, _stepGroundTargetPoints[i]);

                if (distance > _stepSize &&_stepGroundTargetPoints[i] != _bodyAttachedPoints[i].position){
                    _stepGroundTargetPoints[i] = _bodyAttachedPoints[i].position;
            }
        }
    }
    void UpdateFootPositions(){
        for (int i = 0; i < _footCount; i++){
            float distance = Vector3.Distance(_IKFootTargets[i].position, _stepGroundTargetPoints[i]);

            Vector3 startPos = _IKFootTargets[i].position;
            Vector3 targetPos = _stepGroundTargetPoints[i];

            float t = Mathf.InverseLerp(0, _stepSize, distance);

            _IKFootTargets[i].position = Vector3.MoveTowards(startPos, targetPos, Time.deltaTime * _stepSpeed);
            //_IKFootTargets[i].position += Vector3.up * _stepCurve.Evaluate(t) * _stepHeightMultiplier;
        }
    }

    void SnapBodyTargetToGround(){

        RaycastHit hit;

        for (int i = 0; i < _footCount; i++){
            if (Physics.Raycast( _bodyAttachedPoints[i].position + _verticalGroundOffset, Vector3.down, out hit, _groundCheckDst, _groundLayer)){
                _bodyAttachedPoints[i].position = hit.point + new Vector3(0, .1f, 0);
            }
        }
    }

    void UpdateVerticalBodyPosition(){
        Vector3 bodyTargetPosition = Vector3.zero;

        for (int i = 0; i < _footCount; i++){
            bodyTargetPosition += _IKFootTargets[i].position;
        }

        bodyTargetPosition /= _footCount;
        bodyTargetPosition = new Vector3(transform.position.x, bodyTargetPosition.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, bodyTargetPosition + _bodyOffset, Time.deltaTime * _bodyPositionUpdateSpeed);
    }

    void UpdateBodyRotation(){
        if (!_rotateBody)
            return;

        Ray ray = new Ray(transform.position, transform.up * -1);
        
        if(Physics.Raycast(ray, out RaycastHit hit, 10f, _groundLayer)){
            Vector3 targetDir = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            Quaternion targetRot = Quaternion.LookRotation(targetDir, hit.normal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * _bodyRotationSpeed);
        }
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < _footCount; i++){

            Gizmos.color = Color.green;
            
            Gizmos.DrawCube(_bodyAttachedPoints[i].position + _verticalGroundOffset, Vector3.one * .1f);

            Gizmos.color = _footDisplayColor;
            Gizmos.DrawWireCube(_IKFootTargets[i].position, Vector3.one * .1f);
            Gizmos.DrawLine(_bodyAttachedPoints[i].position + _verticalGroundOffset, (_bodyAttachedPoints[i].position + _verticalGroundOffset) + (Vector3.down * _groundCheckDst));

            //Gizmos.color = Color.cyan;
            //Gizmos.DrawLine(_bodyAttachedPoints[i].position, _stepGroundTargetPoints[i]);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_IKFootTargets[i].position, _bodyAttachedPoints[i].position);
            Gizmos.color = Color.green;
            Vector3 dir = (_bodyAttachedPoints[i].position - _IKFootTargets[i].position).normalized;
            Gizmos.DrawLine(_IKFootTargets[i].position, dir * _stepSize + _IKFootTargets[i].position);

        }
    }
}
