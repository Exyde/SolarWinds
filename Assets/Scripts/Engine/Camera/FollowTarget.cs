using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour{

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] bool lookAtTarget;
    
    private void Awake(){
        if (target == null) target = this.transform;
    }

    private void Update(){
        if (lookAtTarget) transform.LookAt(target);
        transform.position = target.position + offset;
    }

    public void SetTarget(Transform target) => this.target = target;
    
    // void OnValidate(){
    //     transform.position = target.position + offset;
    //     if (lookAtTarget) transform.LookAt(target);
    // }

}
