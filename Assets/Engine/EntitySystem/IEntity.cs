using UnityEngine;

namespace Vibrant
{
    public interface IEntity
    {
        public void Birth();
        public void Tick();
        public void Die();

    }
}

