using Vibrant.Core;

namespace Entity
{
    public class Rabbit : Entity
    {
        public override void Die()
        {
            base.Die();
            Logger.Instance.LogMessage("Custom death for Rabbit");
        }
    }
}