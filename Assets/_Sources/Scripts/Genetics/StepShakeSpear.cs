using System.Collections.Generic;
using Engine.Extensions;
using EnhancedEditor;
using UnityEngine;

namespace Genetics
{
    public class StepShakeSpear : MonoBehaviour
    {
        public int iterations = 1000;
        public int populationSize = 1000;
        public string targetString = "hello";
        class DNA
        {
            private char[] _genes;
            private int _length;
            private float _fitness;
            private float mutationRate;

            public float Fitness => _fitness;
            public DNA(int length)
            {
                _length = length;
                _fitness = 0;
                _genes = new char[length];

                mutationRate = 0.01f;
                

                for (int i = 0; i < length; i++)
                {
                    _genes[i] = GetRandomGene();
                }
            }

            public void CalculateFitness(string target)
            {
                float score = 0;

                for (int i = 0; i < _genes.Length; i++)
                {
                    if (_genes[i] == target[i])
                    {
                        score++;
                    }
                }

                _fitness = score / target.Length;
            }
            
            public void Log() => Debug.Log("DNA : " + this.ToString());

            public override string ToString()
            {
                var dna = "[";

                foreach (var gene in _genes)
                {
                    dna += gene;
                }

                dna += "]";

                return dna;
            }

            private char GetRandomGene() => StringExtension.GetRandomChar();

            public void Mutate()
            {
                for(int i = 0; i < _length; i++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                    {
                        _genes[i] = GetRandomGene();
                    }
                }
            }

            public DNA CrossOver(DNA other)
            {
                DNA child = new DNA(_length);

                var midpoint = Mathf.Floor(UnityEngine.Random.Range(0, _length));

                for (int i = 0; i < _length; i++)
                {
                    child._genes[i] = i < midpoint ? _genes[i] : other._genes[i];
                }

                return child;
            }
        }

        private DNA[] _population;
        
        [Button(ActivationMode.Always)]
        public void Simulate()
        {
            Setup();

            foreach (var dna in _population)
            {
                dna.Log();
            }

            for (int i = 0; i < iterations; i++)
            {
                Run();
            }
            
            foreach (var dna in _population)
            {
                dna.Log();
            }
        }

        private void Setup()
        {
            _population = new DNA[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                _population[i] = new DNA(targetString.Length);
            }
        }

        private void Run()
        {
            //Fitness
            foreach (var dna in _population)
            {
                dna.CalculateFitness(targetString);
            }

            //Mating Pool
            List<DNA> matingPool = new List<DNA>();
            
            foreach (var dna in _population)
            {
                var occurence = Mathf.Floor(dna.Fitness * 100);
                
                for (int i = 0; i < occurence; i++)
                {
                    matingPool.Add(dna);
                }
            }

            //Reproduction and Mutation

            for (int i = 0; i < populationSize; i++)
            {
                DNA parentA = matingPool.GetRandomElement();
                DNA parentB = matingPool.GetRandomElement();

                DNA child = parentA.CrossOver(parentB);
                child.Mutate();

                _population[i] = child;
            }
        }
    }
}