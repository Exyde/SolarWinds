using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    public enum DNACrossMode
    {
        Mix
    };
    
    public abstract class DNA
    {
        protected List<Gene> _genes;

        protected float _fitnessScore;

        public List<Gene> Genes => _genes;
        public float FitnessScore => _fitnessScore;

        protected DNA() { }

        /// <summary>
        /// Try to mutate each gene for this DNA
        /// </summary>
        /// <returns></returns>
        public virtual bool Mutate()
        {
            var mutedOnce = false;

            foreach (var gene in _genes)
            {
                if (gene.Mutate())
                {
                    mutedOnce = true;
                }
            }

            return mutedOnce;
        }


        /// <summary>
        /// Create a child by mixing DNA of parents
        /// </summary>
        /// <param name="other"></param>
        /// <param name="crossMode"></param>
        /// <returns></returns>
        public abstract DNA CrossOver(DNA other, DNACrossMode crossMode = DNACrossMode.Mix);

        public abstract float EvaluateFitness(string fitnessTarget);

        public void LogSelf(int index )
        {
            string dna = "DNA [ ";

            foreach (var gene in _genes)
            {
                dna += $" {gene.GetValue()} ";
            }

            dna += " ] -> index : " + index ;
            
            Debug.Log(dna);
        }
    }
}