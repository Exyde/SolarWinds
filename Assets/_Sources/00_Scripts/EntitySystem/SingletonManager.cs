using System.Collections.Generic;
using UnityEngine;

namespace Vibrant.Core
{
    [DefaultExecutionOrder(-99)]
    public abstract class SingletonManager<T1, T2> : MonoBehaviour where T1 : SingletonManager<T1, T2>
    {
        public static T1 Instance = null;

        [SerializeField] private List<T2> _elements = new();
        protected List<T2> Elements => _elements;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T1;
            }
            else
            {
                Destroy(this);
            }
        }

        public virtual void Register(T2 item) => _elements.Add(item);
        public virtual void Unregister(T2 item) => _elements.Remove(item);
        public virtual void Clear() => _elements.Clear();
    }
}