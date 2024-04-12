using System;
using Systems.Entities;
using UnityEngine;

namespace _Sources.Scripts.Player
{
    
    //Utiliser une interface IRidable ?
    public class EntityRidingController : MonoBehaviour
    {

        private bool isRiding;
        [SerializeField] private float detectionRadius = 1f;
        [SerializeField] private LayerMask entityLayer;

        [SerializeField] private Transform attachedEntity;
        private void Update()
        {
            if (Input.GetKey(KeyCode.X))
            {
                ToggleRideState();
            }
        }

        private void LateUpdate()
        {
            if (attachedEntity != null)
            {
                transform.position = attachedEntity.position;
            }
        }

        private void ToggleRideState()
        {
            if (isRiding)
            {
                SetFree();
                return;
            }

            StartRideEntity();
        }

        private void SetFree()
        {
            isRiding = false;
            attachedEntity = null;
        }

        private void StartRideEntity()
        {
            var colliders = Physics.OverlapSphere(transform.position, detectionRadius, entityLayer);
            {
                foreach (var coll in colliders )
                {
                    if (!coll.TryGetComponent(out Entity entity)) continue;
                    AttachTo(entity);
                    break;
                }
            }
        }

        private void AttachTo(Entity entity)
        {
            //Todo : Implement holder interface
            isRiding = true;
            attachedEntity = entity.transform;
        }
    }
}