using UnityEngine;

namespace Core.GameEvents{
    public class OnTriggerStayEvent : OnTriggerGameEvent{
   
        [Space (10)]
        [SerializeField] Timer _timer;

        protected override void SetEventType()
        {
            _eventName = EventName.TRIGGER_STAY; 
        }
        private void OnTriggerStay(Collider other){

            if (_triggerDetectionMode == TriggerDetectionMode.Layer && other.gameObject.layer == _triggerLayer){
                _timer.Update();
            }

            if (_triggerDetectionMode == TriggerDetectionMode.Tag && other.gameObject.tag == _triggerTag){
                _timer.Update();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (_triggerDetectionMode == TriggerDetectionMode.Layer && other.gameObject.layer == _triggerLayer){
                _timer.ResetTimerValue();
            }

            if (_triggerDetectionMode == TriggerDetectionMode.Tag && other.gameObject.tag == _triggerTag){
                _timer.ResetTimerValue();
            }
        }

        private void OnEnable() {
            _timer.OnTimerEnd += base.DispatchEvent;
            Debug.Log("Registered event");
        }

        private void OnDisable() {
            _timer.OnTimerEnd -= base.DispatchEvent;      
        }
    } 
}