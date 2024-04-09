using UnityEngine;

namespace Genetics
{
    public class StringGene : Gene
    {
        private string _value;

        private string Value => _value;

        public static readonly float InnerMutationRate = 1f;
        
        public StringGene(float mutationRate)
        {
            MutationRate = mutationRate;
            _value = StringExtension.GetRandomLetter();
        }

        public override float MutationRate { get; }
        
        public override bool Mutate()
        {
            if (Random.Range(0, 100) < MutationRate)
            {
                _value = StringExtension.GetRandomLetter();
                return true;
            }

            return false;
        }

        public override string GetValue()
        {
            return Value;
        }
    }
}