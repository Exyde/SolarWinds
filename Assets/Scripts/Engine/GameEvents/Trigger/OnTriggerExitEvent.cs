using UnityEngine;

namespace Core.GameEvents{
    public class OnTriggerExitEvent : OnTriggerGameEvent{

        protected override void SetEventType()
        {
            _eventName = EventName.TRIGGER_EXIT; 
        }
        private void OnTriggerExit(Collider other){
            if (_triggerDetectionMode == TriggerDetectionMode.Layer && other.gameObject.layer == _triggerLayer){
                base.DispatchEvent();
            }

            if (_triggerDetectionMode == TriggerDetectionMode.Tag && other.gameObject.tag == _triggerTag){
                base.DispatchEvent();
            }
        }
    }
}
