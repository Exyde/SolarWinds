using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameEvents{

    [System.Serializable]
    public enum EventName{
        TRIGGER_ENTER, TRIGGER_STAY, TRIGGER_EXIT,
        COLLISION_ENTER, COLLISION_STAY,COLLISION_EXIT,
        RAYCAST_LOOK, RAYCAST_INTERACT, RAYCAST_PICTURE,
        ON_NEXT_SEQUENCE, ON_GAME_PAUSE  
    };

    [System.Serializable]
    public class FactCondition{
        [SerializeField] public string _blackboardName = "Blackboard Name";
        [SerializeField] public string _factName = "Fact Name";
        [SerializeField] public Comparaison _comparaison;
        [SerializeField] public int _value = 0;
    }

    [System.Serializable]
    public class FactOperation{
        [SerializeField] public string _blackboardName;
        [SerializeField] public string _factName;
        [SerializeField] public Operation _operation;
        [SerializeField] public int _value;
    }

    
    public enum Operation {SetTo, Add, Substract };
    public enum Comparaison { Equal, Different, Superior, SuperiorOrEqual, Inferior, InferiorOrEqual };

    [System.Serializable]
    public class BlackBoard : ISerializationCallbackReceiver
    {
        [HideInInspector] public string name;

        public string BlackboardName;

        public List<Fact> Facts;

        public Dictionary<string, Fact> DictionaryToFact = new();

        public void OnBeforeSerialize()
        {
            name = "Blackboard " + BlackboardName;
        }

        public void OnAfterDeserialize()
        {
            name = "Blackboard  " + BlackboardName;
        }

        public void OnStart()
        {
            foreach (Fact curentFact in Facts)
            {
                curentFact.ResetValue();

                DictionaryToFact.Add(curentFact.FactName, curentFact);
            }
        }

    }

    [System.Serializable]
    public class Fact : ISerializationCallbackReceiver
    {
        [HideInInspector] public string name;

        public string FactName;

        public int CurrentFactValue;

        public int DefaultFactValue;

        public void ResetValue()
        {
            CurrentFactValue = DefaultFactValue;
        }

        public void OnBeforeSerialize()
        {
            name = FactName;
        }

        public void OnAfterDeserialize()
        {
            name = FactName;
        }

    }
}
