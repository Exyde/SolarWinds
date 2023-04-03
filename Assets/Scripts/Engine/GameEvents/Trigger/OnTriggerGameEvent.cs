using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Core.GameEvents{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class OnTriggerGameEvent : GameEvent {

        protected enum TriggerDetectionMode {Layer, Tag};
        protected enum TriggerMode {Enter, Stay, Exit};
        private Collider coll;

        [Header("Settings")]
        [SerializeField] protected TriggerDetectionMode _triggerDetectionMode = TriggerDetectionMode.Layer;
        [SerializeField] protected LayerMask _triggerLayer;
        [SerializeField] protected string _triggerTag;
 
        void Reset(){
            coll = GetComponent<Collider>();
            if (coll == null){
                coll = gameObject.AddComponent<BoxCollider>();
            }
            coll.isTrigger = true;
        }
    }
}