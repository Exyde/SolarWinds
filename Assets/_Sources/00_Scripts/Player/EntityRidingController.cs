using _Sources.Scripts.EntitySystem;
using UnityEngine;

namespace _Sources.Scripts.Player
{
    public class EntityRidingController : MonoBehaviour
    {

        private bool isRiding;
        [SerializeField] private float detectionRadius = 1f;
        [SerializeField] private LayerMask entityLayer;

        [SerializeField] private IRidable attachedEntity;
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
                transform.position = attachedEntity.AnchorPoint;
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
                    if (!coll.TryGetComponent(out IRidable ridable)) continue;
                    AttachTo(ridable);
                    break;
                }
            }
        }

        private void AttachTo(IRidable ridable)
        {
            isRiding = true;
            attachedEntity = ridable;
        }
    }
}