using Vibrant;
using Vibrant.Core;

namespace Entity
{
    public class Wolf : Entity
    {
        public override void Birth()
        {
            base.Birth();
            Logger.Instance.LogMessage("Custom Birth for Wolf");
        }
    }
}