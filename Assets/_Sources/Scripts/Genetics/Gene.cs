using Random = UnityEngine.Random;

namespace Genetics
{
    public abstract class Gene
    {
       public virtual float MutationRate { get; }

        public virtual bool Mutate()
        {
            return false;
        }

        public static Gene CrossOver(Gene a, Gene b)
        {
            return (new System.Random().Next(100) < 50) ? a : b;
        }

        public abstract string GetValue();

    }
}