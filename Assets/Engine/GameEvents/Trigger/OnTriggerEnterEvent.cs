using UnityEngine;

namespace Core.GameEvents{
    public class OnTriggerEnterEvent : OnTriggerGameEvent 
    {

        protected override void SetEventType()
        {
            eventType = EventType.TRIGGER_ENTER; 
        }
        
        private void OnTriggerEnter(Collider other){
            if (_triggerDetectionMode == TriggerDetectionMode.Layer && other.gameObject.layer == _triggerLayer){
                base.DispatchEvent();
            }

            if (_triggerDetectionMode == TriggerDetectionMode.Tag && other.gameObject.tag == _triggerTag){
                base.DispatchEvent();
            }
        }
    }
}

