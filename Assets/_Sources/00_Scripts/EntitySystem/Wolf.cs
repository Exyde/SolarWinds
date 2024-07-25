using Systems.Entities;

public class Wolf : Entity
{
    public override void Birth()
    {
        base.Birth();
        Engine.Core.Instance.Logger.Log("Custom Birth for Wolf");
    }
}
