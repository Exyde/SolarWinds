using System;
using UnityEngine;
using UnityEngine.Events;

// Source : https://www.youtube.com/watch?v=Qa8QUru6hc0

namespace Engine
{
    [Serializable]
    public class Observer<T>
    {
        [SerializeField] private T _value;
        [SerializeField] private UnityEvent<T> OnValueChanged;

        public static implicit operator T(Observer<T> observer) => observer._value;

        public T Value
        {
            get => _value;
            set => Set(value);
        }
        
        public Observer(T value, UnityAction<T> callback = null)
        {
            this._value = value;
            OnValueChanged = new UnityEvent<T>();
            if (callback != null) OnValueChanged.AddListener(callback);
        }
        
        public void Set(T value)
        {
            if (Equals(this._value, value)) return;
            this._value = value;
            Invoke();
        }

        public void Invoke()
        {
            //todo : Logger
            //Debug.Log($"Invoking {OnValueChanged.GetPersistentEventCount()} listeners for type {nameof(T)} ");
            OnValueChanged.Invoke(_value);
        }

        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            OnValueChanged ??= new UnityEvent<T>();
            OnValueChanged.AddListener(callback);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            if (OnValueChanged == null) return;
            
            OnValueChanged.RemoveListener(callback);
        }

        public void RemoveAllListeners()
        {
            if (OnValueChanged == null) return;
            OnValueChanged.RemoveAllListeners();
        }

        public void Dispose()
        {
            RemoveAllListeners();
            OnValueChanged = null;
            _value = default;
        }
    }
}