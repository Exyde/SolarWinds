using Vibrant.Core;

namespace Entity
{
    public class Rabbit : Entity
    {
        public override void Die()
        {
            base.Die();
            Engine.Core.Instance.Logger.Log("Custom death for Rabbit");
        }
    }
}