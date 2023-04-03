using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Core.GameEvents{
    public abstract class GameEvent : MonoBehaviour {
        
        //Core Game Action - Static, and registered by ALL the managers ?
        public static Action<EventName, string> _onGameEvent;

        [Header("Event Parameters - Rien a rentrer ici ._. - juste debug !")]
        [SerializeField] protected EventName _eventName;
        [SerializeField] protected string _eventSender;

        [Header("Scriptable Events for Designers <3")]
        [SerializeField] List<ScriptableEvents> _scriptableEvents;

        private void Awake() {
            //_eventName = this.GetType().Name;
            SetEventType();
            _eventSender = this.gameObject.name;
        }

        protected virtual void SetEventType(){

        }

        internal virtual void DispatchEvent(){
            Logger.LogInfo("Event Dispatched : " + _eventName + " | Event Sender : " + _eventSender);
            _onGameEvent?.Invoke(_eventName, _eventSender);

            foreach(ScriptableEvents e in _scriptableEvents){
                if (IsEventValid(e.GetFactsConditions())){
                    Logger.LogInfo("All Facts are valids => Triggering Event : " + _eventName);

                    e.GetEvent()?.Invoke();
                    
                    foreach (FactOperation operation in e.GetFactsOperations()){
                        //Todo : Set Fact Value
                    }
                }
            }
        }

        bool IsEventValid(List<FactCondition> _facts){
            foreach(FactCondition factCondition in _facts){
                bool test = true; //Todo : Check facts value !
                if (test == false){
                    Logger.LogInfo("Failed at fact XXX - To Implement Better");
                    return false;
                } else{
                    Logger.LogInfo(factCondition._factName + " @ " + factCondition._value + " is valid !");
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public struct ScriptableEvents{
        [SerializeField] string _eventName;
        [SerializeField] UnityEvent _event;
        [SerializeField] List<FactOperation> _factOperations;
        [SerializeField] List<FactCondition> _factConditions;

        public UnityEvent GetEvent() => _event;
        public List<FactCondition> GetFactsConditions() => _factConditions;
        public List<FactOperation> GetFactsOperations() => _factOperations;
    }
}





