using Vibrant.Core;
using Systems.Entities;

public class Rabbit : Entity
{
    public override void Die()
    {
        base.Die();
        Engine.Core.Instance.Logger.Log("Custom death for Rabbit");
    }
}
