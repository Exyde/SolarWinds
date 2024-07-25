using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    public class CatStringDNA : DNA
    {
        private readonly int _length;
        public CatStringDNA(int length)
        {
            _genes = new List<Gene>();
            _length = length;

            Init();
        }

        private void Init()
        {
            for (var i = 0; i < _length; i++)
            {
                _genes.Add(new StringGene(StringGene.InnerMutationRate));
            }
        }
        
        public override DNA CrossOver(DNA other, DNACrossMode crossMode = DNACrossMode.Mix)
        {
            var child = new CatStringDNA(_length);
            
            for (var i = 0; i < _length; i++)
            {
                child.Genes[i] = Gene.CrossOver(this._genes[i],other.Genes[i]);
            }

            return child;
        }

        public override float EvaluateFitness(string fitnessTarget)
        {
            float score = 0;
            var count = _genes.Count;
            
                for (int i = 0; i < count; i++)
                {
                    var geneValue = _genes[i].GetValue();
                    var fitnessValue = fitnessTarget[i].ToString();

                    if (geneValue == fitnessValue)
                    {
                        score ++;
                    }

                _fitnessScore = score  / count;
            }

            return _fitnessScore;
        }

    }
}