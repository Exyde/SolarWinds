using Vibrant;
using Vibrant.Core;

namespace Entity
{
    public class Wolf : Entity
    {
        public override void Birth()
        {
            base.Birth();
            Engine.Core.Instance.Logger.Log("Custom Birth for Wolf");
        }
    }
}