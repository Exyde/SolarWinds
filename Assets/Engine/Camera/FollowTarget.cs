using UnityEngine;

namespace ExydeToolbox
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;
        [SerializeField] bool lookAtTarget;
    
        private void Awake(){
            if (target == null) target = transform;
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
}

