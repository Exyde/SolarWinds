using System;

namespace Systems.Entities
{
    [Serializable]
    public abstract class EntityBehavior
    {
        public int priority;
        public abstract void Setup();
        public abstract void Process();
    }
}