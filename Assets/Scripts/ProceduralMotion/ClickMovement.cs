using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMovement : MonoBehaviour
{

    [SerializeField] Camera _cam;

    [SerializeField] Vector3 _targetPos;
    [SerializeField] Vector3 _meshSkinSize = new Vector3(0, .5f, 0);
    [SerializeField] LayerMask _groundLayer;

    [SerializeField] Vector3 _direction;
    [SerializeField] float _moveSpeed;

    private void Start() {
        _targetPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) _targetPos = GetNewTargetPosition();

        if (Vector3.Distance(transform.position, _targetPos) > .1f) MoveTowardTargetPosition();
    }

    Vector3 GetNewTargetPosition(){
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer)){
            Vector3 target = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            UpdateDirection(target);
            return target;
        }

        return _targetPos + _meshSkinSize;
    }

    void MoveTowardTargetPosition() => transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);

    void UpdateDirection(Vector3 point) => _direction = (transform.position - point).normalized;
}
