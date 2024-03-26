using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.GameEvents{
    public abstract class GameEvent : MonoBehaviour {
        
        //Core Game Action - Static, and registered by ALL the managers ?
        public static Action<EventType, string> _onGameEvent;

        [FormerlySerializedAs("_eventName")]
        [Header("Event Parameters - Rien a rentrer ici ._. - juste debug !")]
        [SerializeField] protected EventType eventType;
        [SerializeField] protected string _eventSender;

        [Header("Scriptable Events for Designers <3")]
        [SerializeField] List<ScriptableEvent> _scriptableEvents;

        private void Awake() {
            //_eventName = this.GetType().Name;
            SetEventType();
            _eventSender = this.gameObject.name;
        }

        protected virtual void SetEventType(){ }

        internal virtual void DispatchEvent(){
            Engine.Core.Instance.Logger.LogInfo("Event Dispatched : " + eventType + " | Event Sender : " + _eventSender);
            _onGameEvent?.Invoke(eventType, _eventSender);

            foreach(ScriptableEvent e in _scriptableEvents){
                if (IsEventValid(e.GetFactsConditions())){
                    Engine.Core.Instance.Logger.LogInfo("All Facts are valids => Triggering Event : " + eventType);

                    e.Event?.Invoke();
                    
                    foreach (FactOperation operation in e.GetFactsOperations()){
                        //Todo : Set Fact Value
                    }
                }
            }
        }

        bool IsEventValid(List<FactCondition> _facts){
            foreach(FactCondition factCondition in _facts){
                bool test = true; //Todo : Check facts value !
                if (test == false)
                {
                    Engine.Core.Instance.Logger.LogInfo("Failed at fact XXX - To Implement Better");
                    return false;
                } else{
                    Engine.Core.Instance.Logger.LogInfo(factCondition._factName + " @ " + factCondition._value + " is valid !");
                }
            }
            return true;
        }
    }

    [Serializable]
    public struct ScriptableEvent
    {
        [SerializeField] string _eventName;
        [SerializeField] UnityEvent _event;
        [SerializeField] List<FactOperation> _factOperations;
        [SerializeField] List<FactCondition> _factConditions;

        public UnityEvent Event => _event;
        public List<FactCondition> GetFactsConditions() => _factConditions;
        public List<FactOperation> GetFactsOperations() => _factOperations;
    }
}





