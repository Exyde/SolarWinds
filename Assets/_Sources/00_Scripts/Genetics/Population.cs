using System;
using System.Collections.Generic;
using Engine.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Genetics
{
    [Serializable]
    public class Population
    {
        [Serializable]
        public class DNA
        {
            [SerializeField] private char[] _genes;
            [SerializeField] private int _length;
            [SerializeField] private float _fitness;
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

        
        [SerializeField] private DNA[] _individuals;

        [SerializeField] private int _count;
        [SerializeField] private float _mutationRate;
        [SerializeField] private string _targetString;

        public Population(int count, string targetString, float mutationRate = 0.01f)
        {
            _count = count;
            _mutationRate = mutationRate;
            _targetString = targetString;
        }

        public void Init()
        {
            _individuals = new DNA[_count];
            
            for (int i = 0; i < _count; i++)
            {
                _individuals[i] = new DNA(_targetString.Length);
            }  
        }
        public void Log()
        {
            foreach (var dna in _individuals)
            {
                dna.Log();
            }
        }
        public void UpdateFitness()
        {
            foreach (var dna in _individuals)
            {
                dna.CalculateFitness(_targetString);
            }
        }

        private List<DNA> GetMatingPool()
        {
            List<DNA> matingPool = new List<DNA>();
            
            foreach (var dna in _individuals)
            {
                var occurence = Mathf.Floor(dna.Fitness * 100);
                
                for (int i = 0; i < occurence; i++)
                {
                    matingPool.Add(dna);
                }
            }

            return matingPool;
        }

        public void ToNextGeneration()
        {
            var matingPool = GetMatingPool();

            for (int i = 0; i < _count; i++)
            {
                DNA parentA = matingPool.GetRandomElement();
                DNA parentB = matingPool.GetRandomElement();

                DNA child = parentA.CrossOver(parentB);
                child.Mutate();

                _individuals[i] = child;
            }
        }
    }
}